namespace Shared.Models
{
    /// <summary>
    /// Base class for paged, sorted and filtered list requests.
    /// Mirrors the ASP.NET Zero / ABP <c>PagedAndSortedResultRequestDto</c> convention.
    /// </summary>
    public abstract class FilterRequestDto
    {
        public int? CurrentPage { get; set; }

        public int? PageSize { get; set; } = 10;

        /// <summary>
        /// Sort expression in ASP.NET Zero format: <c>"ColumnName [ASC|DESC]"</c>.
        /// Examples: <c>"Name ASC"</c>, <c>"CreatedDateUtc DESC"</c>.
        /// When null or empty the service applies its own default ordering.
        /// </summary>
        public string? Sorting { get; set; }

        public bool? ApplyPagination { get; set; } = false;
    }
}
