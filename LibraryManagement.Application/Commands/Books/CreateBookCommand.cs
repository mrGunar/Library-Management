namespace LibraryManagement.Application.Commands.Books
{
    public record CreateBookCommand
    {
        public string Title { get; init; } = string.Empty;
        public string ISBN { get; init; } = string.Empty;
        public string? Description { get; init; }
        public long AUthorId { get; init; }
        public long CategoryId { get; init; }
        public DateTime? PublishedDate { get; init; }
        public int? PageCount { get; init; }
    }
}
