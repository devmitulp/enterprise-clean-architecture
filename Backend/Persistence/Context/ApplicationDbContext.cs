using System.Linq.Expressions;
using Application.Common.Contexts;
using Domain.Common;
using Domain.Entities.Employees;
using Domain.Entities.JobTitles;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Context
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<JobTitle> JobTitles => Set<JobTitle>();

        public DbSet<Employee> Employees => Set<Employee>();

        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            // Configure global query filter for soft-deleted entities
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(AuditableEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "x");
                    var property = Expression.Property(parameter, nameof(AuditableEntity.IsDeleted));
                    var notExpression = Expression.Not(property);
                    var lambda = Expression.Lambda(notExpression, parameter);

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries<AuditableEntity>();

            var currentUserId = UserContext.UserId;

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedDateUtc = DateTime.UtcNow;
                    entry.Entity.CreatedBy = currentUserId;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedDateUtc = DateTime.UtcNow;
                    entry.Entity.UpdatedBy = currentUserId;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}