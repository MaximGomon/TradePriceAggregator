using PriceAggregator.Common.Processor.Contracts;

namespace PriceAggregator.Common.Processor.Handlers;

public class HandlerResolver : IHandlerResolver
{
    private readonly Dictionary<string, IExchangeHandler> _exchangeHandlers;
    public HandlerResolver(IEnumerable<IExchangeHandler> handlers)
    {
        var exchangeHandlers = handlers.ToList();
        _exchangeHandlers = new Dictionary<string, IExchangeHandler>(exchangeHandlers.Count);

        exchangeHandlers.ForEach(x => _exchangeHandlers.Add(x.Name, x));
    }

    public IExchangeHandler ResolveHandler(string name)
    {
        if (_exchangeHandlers.ContainsKey(name))
        {
            return _exchangeHandlers[name];
        }
        throw new ArgumentException($"Can`t resolve handler with name {name}", "Handler name");
    }
}