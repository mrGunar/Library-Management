namespace LibraryManagement.Application.Commands.Borrowing
{
    public record ReturnBookCommand
    {
        public long BorrowingId { get; init; }
        public DateTime ReturnedDate { get; init; } = DateTime.UtcNow;
    }
}
