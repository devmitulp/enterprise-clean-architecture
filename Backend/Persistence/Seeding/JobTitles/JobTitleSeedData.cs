using Domain.Entities.JobTitles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Seeding.JobTitles
{
    public static class JobTitleSeedData
    {
        public static List<JobTitle> Data => new()
        {
            new JobTitle
            {
                Id = 1,
                Name = "Software Engineer",
                Description = "Develops software applications",
                IsActive = true,
                CreatedDateUtc = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },

            new JobTitle
            {
                Id = 2,
                Name = "Senior Software Engineer",
                Description = "Handles advanced development tasks",
                IsActive = true,
                CreatedDateUtc = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },

            new JobTitle
            {
                Id = 3,
                Name = "Technical Lead",
                Description = "Leads technical implementation",
                IsActive = true,
                CreatedDateUtc = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },

            new JobTitle
            {
                Id = 4,
                Name = "Project Manager",
                Description = "Manages projects and teams",
                IsActive = true,
                CreatedDateUtc = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },

            new JobTitle
            {
                Id = 5,
                Name = "QA Engineer",
                Description = "Performs testing and QA activities",
                IsActive = true,
                CreatedDateUtc = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        };
    }
}