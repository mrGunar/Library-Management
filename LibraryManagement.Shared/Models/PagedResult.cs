namespace LibraryManagement.Shared.Models
{
    public class PagedResult<T>
    {

        public int TotalCount { get; }
        public int PagedSize { get; }
        public int PagedNumber { get; }
        public IReadOnlyCollection<T> Items { get; }

        private PagedResult(IEnumerable<T> items, int totalCount, int pagedNumber, int pageSize)
        {
            TotalCount = totalCount;
            PagedNumber = pagedNumber;
            PagedSize = pageSize;
            Items = items.ToList().AsReadOnly();
        }

        public static PagedResult<T> Create(IEnumerable<T> items, int totalCount, int pagedNumber, int pageSize)
        {
            return new PagedResult<T>(items, totalCount, pagedNumber, pageSize);
        }

    }
}
