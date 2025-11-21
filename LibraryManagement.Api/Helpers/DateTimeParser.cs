using System.Globalization;

namespace LibraryManagement.Api.Helpers
{
    public class DateTimeParser
    {
        public static DateTime? ParseDate(string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;

            return DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var result) ? result : null;
        }
    }
}
