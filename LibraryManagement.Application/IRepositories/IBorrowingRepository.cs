using LibraryManagement.Application.Queries;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Shared.Models;

namespace LibraryManagement.Application.IRepositories
{
    public interface IBorrowingRepository
    {
        Task<Borrowing?> GetByIdAsync(long borrowingId, CancellationToken cancellationToken = default);
        Task<Borrowing?> GetActiveBorrwoingForBookAsync(long borrowingId, CancellationToken cancellationToken = default);
        Task<PagedResult<Borrowing>> GetUserBorrowingsAsync(BorrowingSearchArgs args,  CancellationToken cancellationToken = default);
        Task<PagedResult<Borrowing>> GetOverdueBooksAsync(BorrowingSearchArgs args, CancellationToken cancellationToken = default);
        Task AddAsync(Borrowing borrowing, CancellationToken cancellationToken = default);
        Task UpdateAsync(Borrowing borrowing, CancellationToken cancellationToken= default);
    }
}
