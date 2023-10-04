using Microsoft.Extensions.Logging;
using PriceAggregator.Data.Context.Entities;
using PriceAggregator.Data.Context.Extensions;

namespace PriceAggregator.Data.Context.Seeds;

public class DataSourcesSeed
{
    public async Task SeedAsync(TradeContext context, ILogger logger)
    {
        logger.LogInformation($"{nameof(DataSourcesSeed)} starts");

        try
        {
            context.DataSources.AddIfNotExists(new DataSourceInfo()
            {
                Id = 1,
                Name = "Bitstamp",
                Number = 1,
                HandlerName = "Bitstamp",
                Url = "https://www.bitstamp.net/api/v2/ohlc",
                Step = 3600
            });
            context.DataSources.AddIfNotExists(new DataSourceInfo()
            {
                Id = 2,
                Name = "Bitfinex",
                Number = 2,
                HandlerName = "Bitfinex",
                Url = "https://api-pub.bitfinex.com/v2/candles",
                Step = 3600
            });

            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogError($"{nameof(DataSourcesSeed)} fail. Details: {Environment.NewLine}{ex}");
        }

        logger.LogInformation($"{nameof(DataSourcesSeed)} end");
    }
}