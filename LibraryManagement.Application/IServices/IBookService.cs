using LibraryManagement.Application.Commands.Books;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Queries;
using LibraryManagement.Shared.Models;

namespace LibraryManagement.Application.IServices
{
    public interface IBookService
    {
        /// <summary>
        /// Get single book
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BookDto> GetBookAsync(long id, CancellationToken cancellationToken);

        /// <summary>
        /// Search with pagination
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        Task<PagedResult<BookDto>> GetBooksAsync(BookSearchArgs args, CancellationToken cancellationToken);


        /// <summary>
        /// Create book with validation
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<BookDto> CreateBookAsync(CreateBookCommand command, CancellationToken cancellationToken);

        /// <summary>
        /// Update book
        /// </summary>
        /// <param name="command"></param>
        Task<BookDto> UpdateBookAsync(UpdateBookCommand command, CancellationToken cancellationToken);

        /// <summary>
        /// Soft delete or hard delete
        /// </summary>
        /// <param name="bookId"></param>
        Task DeleteBookAsync(long bookId, CancellationToken cancellationToken);

        /// <summary>
        ///  Check if book is available
        /// </summary>
        /// <param name="bookId"></param>
        Task<bool> CheckAvailabilityAsync(long bookId, CancellationToken cancellationToken);
    }
}
