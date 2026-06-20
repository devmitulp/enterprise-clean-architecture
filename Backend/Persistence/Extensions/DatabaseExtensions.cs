using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Persistence.Context;

namespace Persistence.Extensions
{
    public static class DatabaseExtensions
    {
        public static async Task ApplyMigrationsAndSeedAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<ApplicationDbContext>>();
            var context = services.GetRequiredService<ApplicationDbContext>();

            try
            {
                logger.LogInformation("Verifying database connection...");

                // Can we connect?
                if (await context.Database.CanConnectAsync())
                {
                    logger.LogInformation("Database connection verified successfully.");
                }
                else
                {
                    logger.LogWarning("Database connection could not be established directly. Attempting to create/migrate...");
                }

                // Check for pending migrations
                var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
                var pendingList = pendingMigrations.ToList();

                if (pendingList.Count > 0)
                {
                    logger.LogInformation("Found {Count} pending migrations: {Migrations}. Applying migrations...", pendingList.Count, string.Join(", ", pendingList));

                    // Apply migrations
                    await context.Database.MigrateAsync();

                    logger.LogInformation("Migrations applied successfully.");
                }
                else
                {
                    logger.LogInformation("No pending migrations. Database is up to date.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while verifying or migrating the database.");
                throw;
            }
        }
    }
}
