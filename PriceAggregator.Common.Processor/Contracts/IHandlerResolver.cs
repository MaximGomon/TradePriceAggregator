namespace PriceAggregator.Common.Processor.Contracts;

public interface IHandlerResolver
{
    IExchangeHandler ResolveHandler(string name);
}