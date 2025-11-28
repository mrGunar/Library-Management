using LibraryManagement.Application.Commands.Books;
using LibraryManagement.Application.Queries;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Shared.Models;
using System.Linq.Expressions;

namespace LibraryManagement.Application.IRepositories
{
    public interface IBookRepository
    {
        Task<Book?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<Book> AddAsync(CreateBookCommand book, CancellationToken cancellationToken = default);
        Task<Book?> UpdateAsync(UpdateBookCommand book, CancellationToken cancellationToken = default);
        Task DeleteAsync(Book book, CancellationToken cancellationToken = default);
        Task<PagedResult<Book>> SearchAsync(BookSearchArgs args, CancellationToken cancellationToken = default);
        Task<int> CountAsync(Expression<Func<Book, bool>> predicates, CancellationToken cancellationToken = default);
        Task MarkAsInactiveAsync(long bookId, CancellationToken cancellationToken = default);
        Task UnmarkAsInactiveAsync(long bookId, CancellationToken cancellationToken = default);
    }
}
