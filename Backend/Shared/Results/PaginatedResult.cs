namespace Shared.Results
{
    public class PaginatedResult : Result
    {
        public int PageNumber { get; protected set; }

        public int PageSize { get; protected set; }

        public long TotalRecords { get; protected set; }

        public int TotalPages =>
            PageSize <= 0
                ? 0
                : (int)Math.Ceiling(
                    TotalRecords /
                    (double)PageSize);

        protected PaginatedResult()
        {
        }

        protected PaginatedResult(
            int pageNumber,
            int pageSize,
            long totalRecords,
            string message = "")
            : base(true, message)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalRecords = totalRecords;
        }
    }

    public class PaginatedResult<T>
    : PaginatedResult
    {
        public IReadOnlyCollection<T> Data { get; private set; }
            = Array.Empty<T>();

        private PaginatedResult(
            IReadOnlyCollection<T> data,
            int pageNumber,
            int pageSize,
            long totalRecords,
            string message = "")
            : base(
                pageNumber,
                pageSize,
                totalRecords,
                message)
        {
            Data = data;
        }

        public static PaginatedResult<T> Success(
            IReadOnlyCollection<T> data,
            int pageNumber,
            int pageSize,
            long totalRecords,
            string message = "")
        {
            return new(
                data,
                pageNumber,
                pageSize,
                totalRecords,
                message);
        }
    }
}
