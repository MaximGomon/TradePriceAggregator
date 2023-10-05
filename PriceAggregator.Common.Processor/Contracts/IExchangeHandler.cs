using PriceAggregator.Common.Processor.Models;
using PriceAggregator.Data.Context.Entities;

namespace PriceAggregator.Common.Processor.Contracts;

public interface IExchangeHandler
{
    Task<List<TradePrice>> Handle(ReadDataRequest request, DataSourceInfo dataSourceInfo);
    string Name { get; }
}