using PriceAggregator.Common.Processor.Contracts;
using PriceAggregator.Data.Context.Entities;

namespace PriceAggregator.Common.Processor.Processors;

public class DataAggregationProcessor : IDataAggregationProcessor
{
    public async Task<TradePrice> ProcessData(List<TradePrice> items)
    {
        if (items == null || !items.Any())
            return null;

        var result = new TradePrice()
        {
            Time = items.First().Time,
            Candle = items.First().Candle,
            Price = items.Average(x => x.Price)
        };

        return result;
    }
}