using LibraryManagement.Application.Commands.Borrowing;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Queries;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Shared.Models;

namespace LibraryManagement.Application.IServices
{
    public interface IBorrowingService
    {

        /// <summary>
        /// Borrow book (check availability, set due date)
        /// </summary>
        /// <param name="command"></param>
        Task<BorrowingDto> BorrowBookAsync(BorrowBookCommand command, CancellationToken cancellationToken);
        
        /// <summary>
        /// Return book (update status, calculate fines if applicable)
        /// </summary>
        /// <param name="command"></param>
        Task<BorrowingDto> ReturnBookAsync(ReturnBookCommand command, CancellationToken cancellationToken);

        /// <summary>
        ///  Get user borrowing history ???????????????????????
        /// </summary>
        /// <param name="userId"></param>
        Task<PagedResult<BorrowingDto>> GetUserBorrowingsAsync(BorrowingSearchArgs args, CancellationToken cancellationToken);
        
        /// <summary>
        /// Get all overdue books
        /// </summary>
        /// <returns>List of Books</returns>
        Task<PagedResult<BorrowingDto>> GetOverdueBooksAsync(BorrowingSearchArgs args, CancellationToken cancellationToken);

        /// <summary>
        /// Calculate fine for overdue book (if applicable)
        /// </summary>
        /// <param name="borrowingId"></param>
        /// <returns></returns>
        Task<decimal> CalculateFineAsync(long borrowingId, CancellationToken cancellationToken); 
    }
}
