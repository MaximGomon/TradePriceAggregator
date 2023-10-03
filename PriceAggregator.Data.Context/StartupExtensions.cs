using Microsoft.Extensions.DependencyInjection;

namespace PriceAggregator.Data.Context
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddDataContext(this IServiceCollection services)
        {
            return services;
        }
    }
}