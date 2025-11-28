using AutoMapper;
using FluentValidation;
using LibraryManagement.Application.Commands.Books;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.IRepositories;
using LibraryManagement.Application.IServices;
using LibraryManagement.Application.Queries;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Shared.Exceptions;
using LibraryManagement.Shared.Models;
using Microsoft.Extensions.Logging;


namespace LibraryManagement.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IValidator<CreateBookCommand> _createValidator;
        private readonly IValidator<UpdateBookCommand> _updateValidator;
        private readonly IMapper _mapper;
        private readonly ILogger<BookService> _logger;

        public BookService(
            IBookRepository bookRepository,
            IAuthorRepository authorRepository,
            ICategoryRepository categoryRepository,
            IValidator<CreateBookCommand> createValidator,
            IValidator<UpdateBookCommand> updateValidator,
            IMapper mapper,
            ILogger<BookService> logger
        )
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _categoryRepository = categoryRepository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<bool> CheckAvailabilityAsync(long bookId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<BookDto> CreateBookAsync(CreateBookCommand command, CancellationToken cancellationToken = default)
        {

            await _createValidator.ValidateAndThrowAsync(command, cancellationToken);
            var newBook = await _bookRepository.AddAsync(command, cancellationToken);

            _logger.LogInformation("The book has been added.");

            return _mapper.Map<BookDto>(newBook);
        }

        public async Task DeleteBookAsync(long bookId, CancellationToken cancellationToken = default)
        {
            var book = await _bookRepository.GetByIdAsync(bookId, cancellationToken)
                ?? throw new NotFoundException(nameof(Book), bookId);

            await _bookRepository.DeleteAsync(book, cancellationToken);
            _logger.LogInformation("Book has been deleted.");
        }

        public async Task<BookDto> GetBookAsync(long bookId, CancellationToken cancellationToken = default)
        {
            var book = await _bookRepository.GetByIdAsync(bookId, cancellationToken)
                ?? throw new NotFoundException(nameof(Book), bookId);

            return _mapper.Map<BookDto>(book);
        }

        public async Task<PagedResult<BookDto>> GetBooksAsync(BookSearchArgs args, CancellationToken cancellationToken = default)
        {
            var result = await _bookRepository.SearchAsync(args, cancellationToken);
            var mappedItems = result.Items.Select(_mapper.Map<BookDto>);
            return PagedResult<BookDto>.Create(mappedItems, result.TotalCount, result.PagedNumber, result.PagedSize);
        }

        public async Task<BookDto> UpdateBookAsync(UpdateBookCommand command, CancellationToken cancellationToken = default)
        {
            await _updateValidator.ValidateAndThrowAsync(command, cancellationToken);

            var book = await _bookRepository.GetByIdAsync(command.BookId, cancellationToken)
                ?? throw new NotFoundException(nameof(Book), command.BookId);

            var updatedBook = await _bookRepository.UpdateAsync(command, cancellationToken);
            return _mapper.Map<BookDto>(updatedBook);
        }
    }
}
