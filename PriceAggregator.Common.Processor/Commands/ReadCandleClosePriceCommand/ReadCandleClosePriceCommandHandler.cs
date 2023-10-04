using Microsoft.Extensions.Logging;
using PriceAggregator.Common.Extensions;
using PriceAggregator.Common.Processor.Contracts;
using PriceAggregator.Common.Processor.Models;
using PriceAggregator.Data.Context;
using PriceAggregator.Data.Context.Entities;

namespace PriceAggregator.Common.Processor.Commands.ReadCandleClosePriceCommand;

public class ReadCandleClosePriceCommandHandler : IReadCandleClosePriceCommandHandler
{
    private readonly TradeContext _context;
    private readonly IHandlerResolver _handlerResolver;
    private readonly IDataAggregationProcessor _processor;
    private readonly ILogger<ReadCandleClosePriceCommandHandler> _logger;

    public ReadCandleClosePriceCommandHandler(TradeContext context,
        IHandlerResolver handlerResolver,
        ILogger<ReadCandleClosePriceCommandHandler> logger,
        IDataAggregationProcessor processor)
    {
        _context = context;
        _handlerResolver = handlerResolver;
        _logger = logger;
        _processor = processor;
    }

    public async Task<CandleClosePrice> Handle(ReadCandleClosePriceCommand request, CancellationToken cancellationToken)
    {
        request.Time = request.Time.TrimMinutesAndLess();

        var price = _context.TradePrices.FirstOrDefault(x => x.Time == request.Time && x.Candle == request.Candle);
        if (price != null)
        {
            return new CandleClosePrice()
            {
                Candle = price.Candle,
                Price = price.Price
            };
        }

        var dataSources = _context.DataSources.ToList();
        var prices = new List<TradePrice>(dataSources.Count);

        dataSources.ForEach(async x =>
        {
            try
            {
                var handler = _handlerResolver.ResolveHandler(x.Name);
                prices.Add(await handler.Handle(request.Time, x));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        });

        var resultPrice = await _processor.ProcessData(prices);
        _context.TradePrices.Add(resultPrice);
        await _context.SaveChangesAsync(cancellationToken);

        return new CandleClosePrice()
        {
            Candle = resultPrice.Candle,
            Price = resultPrice.Price
        };
    }
}