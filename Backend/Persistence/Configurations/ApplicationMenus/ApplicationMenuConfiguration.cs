using Domain.Entities.ApplicationMenus;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.ApplicationMenus
{
    public class ApplicationMenuConfiguration : IEntityTypeConfiguration<ApplicationMenu>
    {
        public void Configure(EntityTypeBuilder<ApplicationMenu> builder)
        {
            builder.ToTable("ApplicationMenus");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedNever();

            builder.Property(x => x.ShortCode)
                .HasMaxLength(50);

            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.IconClass)
                .HasMaxLength(100);

            builder.Property(x => x.RouteUrl)
                .HasMaxLength(200);

            builder.Property(x => x.ParentId);
            builder.Property(x => x.DisplayOrder);
            builder.Property(x => x.IsShowInMenu)
                .HasDefaultValue(true);
            builder.Property(x => x.IsShowOnMobile)
                .HasDefaultValue(false);

            // Self‑referencing relationship
            builder.HasOne(x => x.ParentMenu)
                .WithMany(x => x.ChildMenus)
                .HasForeignKey(x => x.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
