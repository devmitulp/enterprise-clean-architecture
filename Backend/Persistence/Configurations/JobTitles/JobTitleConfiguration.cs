using Domain.Entities.JobTitles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Seeding.JobTitles;

namespace Persistence.Configurations.JobTitles
{
    public class JobTitleConfiguration : IEntityTypeConfiguration<JobTitle>
    {
        public void Configure(EntityTypeBuilder<JobTitle> builder)
        {
            builder.ToTable("JobTitles");

            builder.HasKey(x => x.Id);

            // Manual Id
            // Prevent auto increment
            builder.Property(x => x.Id)
                   .ValueGeneratedNever();

            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);

            builder.Property(x => x.CreatedDateUtc)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasData(JobTitleSeedData.Data);
        }
    }
}
