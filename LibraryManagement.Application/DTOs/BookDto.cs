using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.DTOs
{
    public record BookDto
    {
        public long BookId { get; init; }
        public string? Title { get; init; } = string.Empty;
        public string? ISBN { get; init; } = string.Empty;
        public string? Description { get; init; } = string.Empty;
        public long AuthorId { get; init; }
        public Author? Author { get; init; }
        public string AuthorName { get; init; } = string.Empty;
        public long CategoryId { get; init; }
        public Category? Category { get; init; }

        public string CategoryName { get; init; } = string.Empty;
        public DateTime PublishDate { get; init; }
        public DateTime CreatedDate { get; init; }
        public DateTime UpdatedDate { get; init; }
        public int? PageCount { get; init; }
        public bool IsAvailable { get; init; }
    }
}
