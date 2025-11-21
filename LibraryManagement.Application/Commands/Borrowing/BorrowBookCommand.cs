namespace LibraryManagement.Application.Commands.Borrowing
{
    public record BorrowBookCommand
    {
        public long BookId { get; init; }
        public long UserId { get; init; }
        public int BorrowDays { get; init; } = 14;
    }
}
