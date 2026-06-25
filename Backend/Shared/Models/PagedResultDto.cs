namespace Shared.Models
{
    public class PagedResultDto<T>
    {
        public int TotalCount { get; set; }

        public IReadOnlyList<T> Items { get; set; } = Array.Empty<T>();

        public PagedResultDto()
        {
        }

        public PagedResultDto(int totalCount, IReadOnlyList<T> items)
        {
            TotalCount = totalCount;
            Items = items;
        }
    }
}
