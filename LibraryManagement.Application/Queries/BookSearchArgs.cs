using LibraryManagement.Application.Models;

namespace LibraryManagement.Application.Queries
{
    public record BookSearchArgs : PaginationArgs
    {
        public string? SearchString { get; init; }
        public long? AuthorId { get; init; }
        public long? CategoryId { get; init; }
        public bool? IsAvailable { get; init; }
    }
}
