using LibraryManagement.Application.Commands.Authors;
using LibraryManagement.Application.Queries;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Shared.Models;

namespace LibraryManagement.Application.IRepositories
{
    public interface IAuthorRepository
    {
        Task<Author?> GetByIdAsync(long authorId, CancellationToken cancellationToken = default);

        Task<PagedResult<Author>> SearchAsync(AuthorSearchArgs args, CancellationToken cancellationToken = default);

        Task<Author> AddAsync(CreateAuthorCommand author, CancellationToken cancellationToken = default);

        Task<Author?> UpdateAsync(UpdateAuthorCommand author, CancellationToken cancellationToken = default);

        Task<int> GetBookCountAsync(long authorId, CancellationToken cancellationToken = default);
    }
}
