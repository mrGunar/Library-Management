using LibraryManagement.Application.Queries;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Shared.Models;

namespace LibraryManagement.Application.IRepositories
{
    public interface IAuthorRepository
    {
        Task<Author?> GetByIdAsync(long authorId, CancellationToken cancellationToken = default);

        Task<PagedResult<Author>> SearchAsync(AuthorSearchArgs args, CancellationToken cancellationToken = default);

        Task AddAsync(Author author, CancellationToken cancellationToken = default);
        
        Task UpdateAsync(Author author, CancellationToken cancellationToken = default);

        Task<int> GetBookCountAsync(long authorId, CancellationToken cancellationToken= default);
    }
}
