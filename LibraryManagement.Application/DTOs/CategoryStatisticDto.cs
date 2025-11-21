namespace LibraryManagement.Application.DTOs
{
    public record CategoryStatisticDto
    {
        public long CategoryId { get; init; }
        public int BookCount { get; init; }
    }
}
