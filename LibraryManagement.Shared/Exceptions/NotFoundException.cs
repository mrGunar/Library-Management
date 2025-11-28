namespace LibraryManagement.Shared.Exceptions
{
    public class NotFoundException : Exception
    {
        public string ResName { get; }
        public object ResId { get; }

        public NotFoundException(string res, object resId)
            : base($"Resource `{res}` `{resId}` was not found.")
        {
            ResName = res;
            ResId = resId;
        }
    }
}
