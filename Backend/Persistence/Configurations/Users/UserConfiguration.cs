using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Seeding.Users;

namespace Persistence.Configurations.Users
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Email)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.PasswordHash)
                .IsRequired();

            builder.HasIndex(x => x.UserName)
                .IsUnique();

            builder.HasIndex(x => x.Email)
                .IsUnique();

            // 1 : 1 Relationship
            builder.HasOne(x => x.Employee)
                .WithOne(x => x.User)
                .HasForeignKey<User>(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(UserSeedData.Data);
        }
    }
}
