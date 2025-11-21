namespace LibraryManagement.Application.Commands.Authors
{
    public record UpdateAuthorCommand
    {
        public long AuthorId { get; init; }
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string? Biography { get; init; } = string.Empty;
        public DateTime? DateOfBirth { get; init; }

        public bool IsActive { get; init; }
    }
}
