using LibraryManagement.Domain.Enum;

namespace LibraryManagement.Api.Helpers
{
    public class StatusParser
    {
        public static BorrowingStatus? ParseStatus(string status)
        {
            return Enum.TryParse<BorrowingStatus>(status, out var parsed) ? parsed : null;
        }
    }
}
