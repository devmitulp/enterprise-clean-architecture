using Domain.Common;

namespace Domain.Entities.ApplicationMenus
{
    public class ApplicationMenu : AuditableEntity
    {
        public string ShortCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? IconClass { get; set; }
        public string? RouteUrl { get; set; }
        public int? ParentId { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsShowInMenu { get; set; }
        public bool IsShowOnMobile { get; set; }
        public ApplicationMenu? ParentMenu { get; set; }
        public ICollection<ApplicationMenu> ChildMenus { get; set; } = new List<ApplicationMenu>();
    }
}
