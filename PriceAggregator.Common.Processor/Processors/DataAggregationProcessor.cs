using PriceAggregator.Common.Processor.Contracts;
using PriceAggregator.Data.Context.Entities;

namespace PriceAggregator.Common.Processor.Processors;

public class DataAggregationProcessor : IDataAggregationProcessor
{
    public async Task<List<TradePrice>> ProcessData(List<TradePrice> items)
    {
        if (items == null || !items.Any())
            return null;

        var result = items.GroupBy(x => x.TimeStamp)
            .Select(x => new TradePrice()
            {
                Candle = x.First().Candle,
                TimeStamp = x.Key,
                Price = x.Average(a => a.Price)
            })
            .ToList();

        return result;
    }
}