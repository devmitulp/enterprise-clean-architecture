namespace Domain.Common
{
    public abstract class AuditableEntity: BaseEntity
    {
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDateUtc { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedDateUtc { get; set; }

        public int? UpdatedBy { get; set; }

        public bool IsDeleted { get; set; }
    }
}
