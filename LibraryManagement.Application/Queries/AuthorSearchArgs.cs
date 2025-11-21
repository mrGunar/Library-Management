using LibraryManagement.Application.Models;

namespace LibraryManagement.Application.Queries
{
    public record AuthorSearchArgs : PaginationArgs
    {
        // TODO: Smth else?
        public string? SearchString { get; init; }
        public bool? IsActive { get; init; }
    }
}
