namespace Shared.Models
{
    public abstract class FilterRequestDto
    {
        public int? CurrentPage { get; set; }

        public int? PageSize { get; set; } = 0;

        public string SortBy { get; set; } = "Description";

        public bool? SortAscending { get; set; } = true;

        public bool? ApplyPagination { get; set; } = false;
    }
}
