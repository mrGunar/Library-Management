using System.Linq.Expressions;
using LibraryManagement.Application.Queries;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Shared.Models;

namespace LibraryManagement.Application.IRepositories
{
    public interface IBookRepository
    {
        Task<Book?> GetByIdAsync(long id,  CancellationToken cancellationToken = default);
        Task AddAsync (Book book, CancellationToken cancellationToken = default);
        Task UpdateAsync(Book book, CancellationToken cancellationToken = default);
        Task DeleteAsync(Book book, CancellationToken cancellationToken= default);
        Task<PagedResult<Book>> SearchAsync(BookSearchArgs args, CancellationToken cancellationToken = default);
        Task<int> CountAsync(Expression<Func<Book, bool>> predicates, CancellationToken cancellationToken = default);

    }
}
