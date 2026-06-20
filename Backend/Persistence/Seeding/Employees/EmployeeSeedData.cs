using Domain.Entities.Employees;
using System;

namespace Persistence.Seeding.Employees
{
    public static class EmployeeSeedData
    {
        public static List<Employee> Data => new()
        {
            new Employee
            {
                Id = 1,
                EmployeeCode = "EMP001",
                FirstName = "Mitul",
                LastName = "Patel",
                Email = "mitul@company.com",
                JobTitleId = 1,
                CreatedDateUtc =
                    new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        };
    }
}
