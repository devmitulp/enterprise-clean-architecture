using Domain.Entities.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Seeding.Employees;

namespace Persistence.Configurations.Employees
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.EmployeeCode)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.LastName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Email)
                .HasMaxLength(200)
                .IsRequired();

            builder.HasIndex(x => x.EmployeeCode)
                .IsUnique();

            builder.HasIndex(x => x.Email)
                .IsUnique();

            builder.HasOne(x => x.JobTitle)
                .WithMany()
                .HasForeignKey(x => x.JobTitleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(EmployeeSeedData.Data);
        }
    }
}
