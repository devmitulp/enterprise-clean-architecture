using Domain.Entities.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.Roles
{
    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.ToTable("RolePermissions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .ValueGeneratedOnAdd();

            // Note: Inherits from BaseEntity, not AuditableEntity, so IsActive and CreatedDateUtc are excluded

            // Relationships
            builder.HasOne(rp => rp.Role)
                   .WithMany(r => r.RolePermissions)
                   .HasForeignKey(rp => rp.RoleId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(rp => rp.ApplicationMenu)
                   .WithMany()
                   .HasForeignKey(rp => rp.ApplicationMenuId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Indexes & Unique Constraints
            builder.HasIndex(rp => new { rp.RoleId, rp.ApplicationMenuId })
                   .IsUnique();
        }
    }
}
