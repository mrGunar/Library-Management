using AutoMapper;
using FluentValidation;
using LibraryManagement.Application.Commands.Borrowing;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.IRepositories;
using LibraryManagement.Application.IServices;
using LibraryManagement.Application.Models;
using LibraryManagement.Application.Queries;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enum;
using LibraryManagement.Shared.Exceptions;
using LibraryManagement.Shared.Models;
using Microsoft.Extensions.Logging;

namespace LibraryManagement.Application.Services
{
    public class BorrowingService : IBorrowingService
    {
        private readonly IBorrowingRepository _borrowingRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IValidator<BorrowBookCommand> _borrowValidator;
        private readonly IValidator<ReturnBookCommand> _returnValidator;
        private readonly IMapper _mapper;
        private readonly ILogger<BorrowingService> _logger;

        /// <summary>
        /// Fine for the one day after deadline
        /// </summary>
        private const decimal DailyFineAmount = 42;

        public BorrowingService(
            IBorrowingRepository borrowingRepository,
            IBookRepository bookRepository,
            IValidator<BorrowBookCommand> borrowValidator,
            IValidator<ReturnBookCommand> returnValidator,
            IMapper mapper,
            ILogger<BorrowingService> logger)
        {
            _borrowingRepository = borrowingRepository;
            _bookRepository = bookRepository;
            _borrowValidator = borrowValidator;
            _returnValidator = returnValidator;
            _mapper = mapper;
            _logger = logger;
        }

        private Task<decimal> CalculateFinePrivateAsync(Borrowing borrowing)
        {
            return Task.FromResult(CalculateFine(borrowing));
        }

        private decimal CalculateFine(Borrowing borrowing)
        {
            var baseline = borrowing.ReturnedDate ?? DateTime.UtcNow;
            if (baseline <= borrowing.DueDate)
            {
                return 0m;
            }

            var overdueDays = (baseline.Date - borrowing.DueDate.Date).Days;
            if (overdueDays <= 0)
            {
                return 0m;
            }

            return overdueDays * DailyFineAmount;
        }

        public async Task<BorrowingDto> BorrowBookAsync(BorrowBookCommand command, CancellationToken cancellationToken = default)
        {
            await _borrowValidator.ValidateAndThrowAsync(command, cancellationToken);

            var book = await _bookRepository.GetByIdAsync(command.BookId, cancellationToken)
                ?? throw new NotFoundException(nameof(Borrowing), command.BookId);

            if (!book.IsAvailable)
            {
                throw new Exception("The book is not available for borrowing.");
            }

            var newBorrowing = await _borrowingRepository.AddAsync(command, cancellationToken);

            await _bookRepository.MarkAsInactiveAsync(command.BookId, cancellationToken);

            _logger.LogInformation("The book has been borrowed.");
            return _mapper.Map<BorrowingDto>(newBorrowing);
        }

        public async Task<decimal> CalculateFineAsync(long borrowingId, CancellationToken cancellationToken = default)
        {
            var borrowing = await _borrowingRepository.GetByIdAsync(borrowingId, cancellationToken)
                ?? throw new NotFoundException(nameof(Borrowing), borrowingId);

            return await CalculateFinePrivateAsync(borrowing);
        }

        public async Task<PagedResult<BorrowingDto>> GetOverdueBooksAsync(BorrowingSearchArgs args, CancellationToken cancellationToken = default)
        {
            var overdue = new PaginationArgs
            {
                PageNumber = args.PageNumber,
                PageSize = args.PageSize,
            };

            var result = await _borrowingRepository.GetOverdueBooksAsync(args, cancellationToken);

            var mappedItems = result.Items.Select(async borrowing =>
            {
                var dto = _mapper.Map<BorrowingDto>(borrowing);
                // TODO: calculate fine probably async
                var fine = await CalculateFinePrivateAsync(borrowing);
                return dto with { FineAmount = fine };
            }).ToArray();

            var resolved = await Task.WhenAll(mappedItems);
            return PagedResult<BorrowingDto>.Create(resolved, result.TotalCount, result.PagedNumber, result.PagedSize);

        }

        public async Task<PagedResult<BorrowingDto>> GetUserBorrowingsAsync(BorrowingSearchArgs args, CancellationToken cancellationToken = default)
        {
            var result = await _borrowingRepository.GetUserBorrowingsAsync(args, cancellationToken);

            var mappedItems = result.Items.Select(async borrwoing =>
            {
                var dto = _mapper.Map<BorrowingDto>(borrwoing);
                // TODO: calculate fine
                var fine = await CalculateFinePrivateAsync(borrwoing);
                return dto with { FineAmount = fine };
            }).ToArray();

            var resolved = await Task.WhenAll(mappedItems);
            return PagedResult<BorrowingDto>.Create(resolved, result.TotalCount, result.PagedNumber, result.PagedSize);

        }

        public async Task<BorrowingDto> ReturnBookAsync(ReturnBookCommand command, CancellationToken cancellationToken = default)
        {
            await _returnValidator.ValidateAndThrowAsync(command, cancellationToken);

            var borrowing = await _borrowingRepository.GetByIdAsync(command.BorrowingId, cancellationToken)
                ?? throw new NotFoundException(nameof(Borrowing), command.BorrowingId);

            if (borrowing.Status == BorrowingStatus.Returned)
            {
                return _mapper.Map<BorrowingDto>(borrowing);
            }


            var book = await _bookRepository.GetByIdAsync(borrowing.BookId, cancellationToken)
                ?? throw new NotFoundException(nameof(Book), borrowing.BookId);


            await _borrowingRepository.UpdateAsync(command, cancellationToken);

            await _bookRepository.UnmarkAsInactiveAsync(book.BookId, cancellationToken);

            var fine = await CalculateFineAsync(borrowing.BorrowingId, cancellationToken);
            return _mapper.Map<BorrowingDto>(borrowing) with { FineAmount = fine };

        }
    }
}