namespace LibraryManagement.Application.Commands.Categories
{
    public record CreateCategoryCommand
    {
        public string Name { get; init; } = string.Empty;
        public string? Description { get; init; } = string.Empty;
        public long? ParentCategory { get; init; }
        public int SortOrder { get; init; }
    }
}
