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
            // TODO: Something brilliant
            return 0;
        }

        public async Task<BorrowingDto> BorrowBookAsync(BorrowBookCommand command, CancellationToken cancellationToken = default)
        {
            await _borrowValidator.ValidateAndThrowAsync(command, cancellationToken);

            var book = await _bookRepository.GetByIdAsync(command.BookId, cancellationToken)
                ?? throw new NotFoundException(nameof(Borrowing), command.BookId);

            if (!book.IsAvailable)
            {
                // TODO: Create a new exception for this case in the shared module
                throw new Exception("The book is not available for borrowing.");
            }

            // TODO: A new exception
            var bor = await _borrowingRepository.GetActiveBorrwoingForBookAsync(command.BookId, cancellationToken)
                ?? throw new Exception("The book already has an active borrowing.");

            var borrowDate = DateTime.UtcNow;
            var dueDate = borrowDate.AddDays(command.BorrowDays <= 0 ? 14 : command.BorrowDays);

            var borrowing = new Borrowing(command.BookId, command.UserId, borrowDate, dueDate);

            // TODO: Marked book as borrowed
            await _borrowingRepository.AddAsync(borrowing, cancellationToken);
            // TODO: Update timestamp in the book
            //await _bookRepository.UpdateAsync(book, cancellationToken);

            _logger.LogInformation("The book has been borrowed.");
            return _mapper.Map<BorrowingDto>(borrowing);
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

            // TODO: Mark as returned

            var book = await _bookRepository.GetByIdAsync(borrowing.BookId, cancellationToken)
                ?? throw new NotFoundException(nameof(Book), borrowing.BookId);

            await _borrowingRepository.UpdateAsync(borrowing, cancellationToken);
            // TODO: Update timestamp in the book
            //await _bookRepository.UpdateAsync(book, cancellationToken);

            // TODO: calculate fine
            var fine = 0;
            return _mapper.Map<BorrowingDto>(borrowing) with { FineAmount = fine };

        }
    }
}