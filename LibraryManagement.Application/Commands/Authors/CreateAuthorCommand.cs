namespace LibraryManagement.Application.Commands.Authors
{
    public record CreateAuthorCommand
    {
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string? Biography { get; init; } = string.Empty;
        public DateTime? DateOfBirth { get; init; }
    }
}
