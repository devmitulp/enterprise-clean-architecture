using Domain.Common;
using Domain.Entities.JobTitles;
using Domain.Entities.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Employees
{
    public class Employee : AuditableEntity
    {
        public string EmployeeCode { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public int JobTitleId { get; set; }

        // Navigation Property
        [ForeignKey(nameof(JobTitleId))]
        public JobTitle JobTitle { get; set; } = default!;

        // Navigation
        public User? User { get; set; }
    }
}
