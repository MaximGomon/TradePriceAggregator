using PriceAggregator.Data.Context.Entities;

namespace PriceAggregator.Common.Processor.Contracts;

public interface IDataAggregationProcessor
{
    Task<List<TradePrice>> ProcessData(List<TradePrice> items);
}