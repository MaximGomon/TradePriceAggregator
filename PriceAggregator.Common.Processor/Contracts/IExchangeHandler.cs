using PriceAggregator.Data.Context.Entities;

namespace PriceAggregator.Common.Processor.Contracts;

public interface IExchangeHandler
{
    Task<TradePrice> Handle(DateTime time, DataSourceInfo dataSourceInfo);
    string Name { get; }
}