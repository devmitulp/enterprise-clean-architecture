using Shared.Models;

namespace Application.Features.JobTitles.DTOs
{
    public class JobTitleDto : AuditableEntityDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
