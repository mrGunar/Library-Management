using LibraryManagement.Application.Models;

namespace LibraryManagement.Application.Queries
{
    public record AuthorSearchArgs : PaginationArgs
    {
        public string? SearchString { get; init; }
        public bool? IsActive { get; init; }
    }
}
