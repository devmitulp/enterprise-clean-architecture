using Domain.Entities.Users;

namespace Persistence.Seeding.Users
{
    public static class UserSeedData
    {
        public static List<User> Data => new()
        {
           new User
           {
               Id = 1,
               EmployeeId = 1,
               UserName = "admin",
               Email = "admin@company.com",
               PasswordHash = "AQAAAAIAAYagAAAAEOEICcDos33D5KeqhPKlST+y37hWet2yDs9KQL4GLEWLrhiJZ4EFQtr5uBkOVtGpzw==",
               IsActive = true,
               CreatedDateUtc =
                   new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
           }
        };
    }
}
