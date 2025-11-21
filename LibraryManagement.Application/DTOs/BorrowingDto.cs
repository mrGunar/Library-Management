using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enum;

namespace LibraryManagement.Application.DTOs
{
    public record BorrowingDto
    {
        public long BorrowingId { get; init; }
        public long BookId { get; init; }
        public string BookTitle { get; init; } = string.Empty;
        public Book? Book { get; init; } // ???
        public long UserId { get; init; }
        public DateTime BorrowDate { get; init; }
        public DateTime DueDate { get; init; }
        public DateTime? ReturnedDate { get; init; }
        public BorrowingStatus Status { get; init; }
        public decimal FineAmount { get; init; }
    }
}
