namespace LibraryManagement.Application.Commands.Books
{
    public record UpdateBookCommand
    {
        public long BookId { get; init; }
        public string Title { get; init; } = string.Empty;
        public string ISBN { get; init; } = string.Empty;
        public string? Description { get; init; } = string.Empty;
        public long AuthorId { get; init; }
        public long CategoryId { get; init; }
        public DateTime? PublishedDate { get; init; }
        public int? PageCount { get; init; }
        public bool? IsAvailable { get; init; }
    }
}
