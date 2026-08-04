using Domain.Entities.Roles;

namespace Persistence.Seeding.Roles
{
    public static class UserRoleSeedData
    {
        public static List<UserRole> Data => new()
        {
            new UserRole
            {
                Id = 1,
                UserId = 1,
                RoleId = 1,
                IsActive = true,
                CreatedDateUtc = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        };
    }
}
