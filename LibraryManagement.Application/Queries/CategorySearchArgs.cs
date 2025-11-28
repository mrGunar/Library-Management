using LibraryManagement.Application.Models;

namespace LibraryManagement.Application.Queries
{
    public record CategorySearchArgs : PaginationArgs
    {
        public string? SearchString { get; init; }
        public long? ParentCategoryId { get; init; }
        public bool? IsActive { get; init; }

    }
}

