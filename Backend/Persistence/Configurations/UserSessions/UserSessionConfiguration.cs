using Domain.Entities.Users;
using Domain.Entities.UserSessions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.UserSessions
{
    public class UserSessionConfiguration : IEntityTypeConfiguration<UserSession>
    {
        public void Configure(EntityTypeBuilder<UserSession> builder)
        {
            builder.ToTable("UserSessions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.AccessToken)
                .IsRequired();

            builder.Property(x => x.RefreshToken)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.UserAgent)
                .HasMaxLength(500);

            builder.Property(x => x.TimeZone)
                .HasMaxLength(100);

            // One User has Many UserSessions
            builder.HasOne(x => x.User)
                .WithMany(x => x.UserSessions)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
