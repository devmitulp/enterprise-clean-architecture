using Domain.Common;

namespace Domain.Entities.JobTitles
{
    public class JobTitle: AuditableEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
