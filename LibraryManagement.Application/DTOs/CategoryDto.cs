using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.DTOs
{
    public record CategoryDto
    {
        public long CategoryId { get; init; }

        public required string Name { get; init; } = string.Empty;

        public string? Description { get; init; } = string.Empty;

        public long? ParentCategoryId { get; init; }

        public Category? ParentCategory { get; init; } // ???

        public int SortOrder { get; init; }

        public bool IsActive { get; init; }

        public int BookCount { get; init; }

        public IReadOnlyCollection<CategoryDto> SubCategories { get; init; } = Array.Empty<CategoryDto>();
    }
}
