using LibraryManagement.Application.Models;
using LibraryManagement.Domain.Enum;

namespace LibraryManagement.Application.Queries
{
    public record BorrowingSearchArgs : PaginationArgs
    {
        public long? UserId { get; init; }
        public BorrowingStatus? BorrowStatus { get; init; }
        public bool OverdueOnly => BorrowStatus == BorrowingStatus.Overdue;
    }
}
