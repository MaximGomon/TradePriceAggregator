using PriceAggregator.Data.Context.Entities;

namespace PriceAggregator.Common.Processor.Contracts;

public interface IDataAggregationProcessor
{
    Task<TradePrice> ProcessData(List<TradePrice> items);
}