using Domain.Entities.Roles;
using Domain.Enums;

namespace Persistence.Seeding.Roles
{
    public static class RoleSeedData
    {
        public static List<Role> Data => new()
        {
            new Role
            {
                Id = 1,
                Name = "Product Admin",
                Description = "System administrator with full access to all features.",
                RoleType = RoleType.ProductAdmin,
                IsActive = true,
                CreatedDateUtc = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        };
    }
}
