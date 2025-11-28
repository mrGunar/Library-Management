namespace LibraryManagement.Application.DTOs
{
    public record AuthorDto
    {
        public long AuthorId { get; init; }
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string? Biography { get; init; } = string.Empty;
        public DateTime? DateOfBirth { get; init; }
        public bool IsActive { get; init; }
        public int BookCount { get; init; }
        public DateTime CreatedDate { get; init; }
        public DateTime UpdatedDate { get; init; }
    }
}
