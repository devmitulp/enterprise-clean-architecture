using Domain.Common;
using Domain.Entities.ApplicationMenus;

namespace Domain.Entities.Roles
{
    public class RolePermission : BaseEntity
    {
        public int RoleId { get; set; }
        
        public int ApplicationMenuId { get; set; }
        
        public bool CanView { get; set; }
        
        public bool CanEdit { get; set; }
        
        public bool CanReview { get; set; }

        public Role Role { get; set; } = default!;
        
        public ApplicationMenu ApplicationMenu { get; set; } = default!;
    }
}
