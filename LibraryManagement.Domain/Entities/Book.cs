namespace LibraryManagement.Domain.Entities
{
    public class Book
    {

        private Book() { }

        public Book(
            string title,
            string isbn,
            long authorId,
            long categoryId,
            string? description = null,
            DateTime? publishDate = null,
            int? pageCount = null)
        {
            Title = title;
            ISBN = isbn;
            AuthorId = authorId;
            CategoryId = categoryId;
            Description = description;
            IsAvailable = true;
            PageCount = pageCount;
            PublishDate = publishDate;

        }

        public long BookId { get; set; }
        public string? Title { get; set; }
        public string? ISBN { get; set; }
        public string? Description { get; set; } = string.Empty;
        public long AuthorId { get; set; }
        public Author? Author { get; set; }
        public long CategoryId { get; set; }
        public Category? Category { get; set; }
        public DateTime? PublishDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set; }
        public int? PageCount { get; set; }
        public bool IsAvailable { get; set; }
    }
}
