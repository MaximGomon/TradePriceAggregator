using Microsoft.EntityFrameworkCore;

namespace TradePriceAggregator;

public static class WebHostExtensions
{
    public static IHost MigrateDbContext<TContext>(
        this IHost webHost,
        Action<TContext, IServiceProvider> seeder)
        where TContext : DbContext
    {
        using (var scope = webHost.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            var logger = services.GetRequiredService<ILogger<TContext>>();

            var context = services.GetService<TContext>();

            try
            {
                logger.LogInformation($"Migrating database associated with context {typeof(TContext).Name}");

                context.Database.Migrate();
                seeder(context, services);

                logger.LogInformation($"Migrated database associated with context {typeof(TContext).Name}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error occurred migrating the database with context {typeof(TContext).Name}");
            }
        }

        return webHost;
    }
}
