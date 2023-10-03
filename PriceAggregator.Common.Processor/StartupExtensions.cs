using Microsoft.Extensions.DependencyInjection;

namespace PriceAggregator.Common.Processor
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddProcessors(this IServiceCollection services)
        {
            return services;
        }

    }
}