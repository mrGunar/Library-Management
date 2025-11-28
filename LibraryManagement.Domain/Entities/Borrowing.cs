using LibraryManagement.Domain.Enum;

namespace LibraryManagement.Domain.Entities
{
    public class Borrowing
    {
        private Borrowing() { }

        public Borrowing(long bookId, long userId, DateTime borrowDate, DateTime dueDate)
        {
            BookId = bookId;
            UserId = userId;
            BorrowDate = borrowDate;
            DueDate = dueDate;
            Status = BorrowingStatus.Active;
        }

        public long BorrowingId { get; set; }
        public long BookId { get; set; }
        public Book? Book { get; set; }
        public long UserId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
        public BorrowingStatus Status { get; set; }
    }
}
