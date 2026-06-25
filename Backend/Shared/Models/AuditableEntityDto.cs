namespace Shared.Models
{
    public abstract class AuditableEntityDto
    {
        public bool IsActive { get; set; } = true;

        public DateTime CreatedDateUtc { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedDateUtc { get; set; }

        public int? UpdatedBy { get; set; }
    }
}
