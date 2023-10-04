using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PriceAggregator.Data.Context.Extensions
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddTradeDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TradeContext>(options =>
                options.UseNpgsql(configuration.GetValue<string>("ConnectionStrings::TradeDb")));

            return services;
        }
    }
}