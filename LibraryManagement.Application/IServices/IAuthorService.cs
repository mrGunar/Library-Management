using LibraryManagement.Application.Queries;
using LibraryManagement.Application.Commands.Authors;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Shared.Models;

namespace LibraryManagement.Application.IServices
{
    public interface IAuthorService
    {
        Task<AuthorDto> GetAuthorAsync(long authorId, CancellationToken cancellationToken);

        Task<PagedResult<AuthorDto>> GetAuthorsAsync(AuthorSearchArgs args, CancellationToken cancellationToken);

        Task<AuthorDto> CreateAuthorAsync(CreateAuthorCommand command, CancellationToken cancellationToken);

        Task<AuthorDto> UpdateAuthorAsync(UpdateAuthorCommand command, CancellationToken cancellationToken);

        Task<int> GetAuthorBookCountAsync(long authorId, CancellationToken cancellationToken);


    }
}

