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

    public async Task<List<CandleClosePrice>> Handle(ReadCandleClosePriceCommand request, CancellationToken cancellationToken)
    {
        request.Start = request.Start.TrimMinutesAndLess();

        //todo check if exists all records for each hour not for any hour
        var prices = _context.TradePrices.Where(x => x.TimeStamp > request.Start.ToEpochTime() 
                                                     && x.TimeStamp <= request.End.ToEpochTime() && x.Candle == request.Candle);
        if (prices.Any())
        {
            return prices.Select(x => new CandleClosePrice()
                {
                    Candle = x.Candle,
                    Price = x.Price,
                    TimeStamp = x.TimeStamp
                })
                .ToList();
        }

        var dataSources = _context.DataSources.ToList();
        var result = new List<TradePrice>(dataSources.Count);

        foreach (var sourceInfo in dataSources)
        {
            try
            {
                var handler = _handlerResolver.ResolveHandler(sourceInfo.Name);
                var data = await handler.Handle(new ReadDataRequest()
                {
                    Candle = request.Candle,
                    End = request.End,
                    Start = request.Start
                }, sourceInfo);

                result.AddRange(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
      
        var resultPrices = await _processor.ProcessData(result);
        _context.TradePrices.AddRange(resultPrices);
        await _context.SaveChangesAsync(cancellationToken);

        return resultPrices.Select(x => new CandleClosePrice()
            {
                Candle = x.Candle,
                Price = x.Price,
                TimeStamp = x.TimeStamp
            })
            .ToList();
    }
}