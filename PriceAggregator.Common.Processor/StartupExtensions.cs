using Microsoft.Extensions.DependencyInjection;
using PriceAggregator.Common.Processor.Clients;
using PriceAggregator.Common.Processor.Commands.ReadCandleClosePriceCommand;
using PriceAggregator.Common.Processor.Contracts;
using PriceAggregator.Common.Processor.Handlers;
using PriceAggregator.Common.Processor.Models;
using PriceAggregator.Common.Processor.Processors;

namespace PriceAggregator.Common.Processor
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddProcessors(this IServiceCollection services)
        {
            services.AddTransient<IHandlerResolver, HandlerResolver>();
            services.AddTransient<IExchangeHandler, BitfinexExchangeHandler>();
            services.AddTransient<IExchangeHandler, BitstampExchangeHandler>();

            services.AddTransient<IExchangeHttpClient<TradeRequestParameter, BitstampPrice>, BitstampHttpClient>();
            services.AddTransient<IExchangeHttpClient<TradeRequestParameter, List<BitfinexPrice>>, BitfinexHttpClient>();

            services.AddTransient<IReadCandleClosePriceCommandHandler, ReadCandleClosePriceCommandHandler>();

            services.AddTransient<IDataAggregationProcessor, DataAggregationProcessor>();

            return services;
        }

    }
}