using PriceAggregator.Common.Extensions;
using PriceAggregator.Common.Processor.Contracts;
using PriceAggregator.Common.Processor.Models;
using PriceAggregator.Data.Context.Entities;

namespace PriceAggregator.Common.Processor.Handlers;

public class BitstampExchangeHandler : IExchangeHandler
{
    private readonly IExchangeHttpClient<TradeRequestParameter, BitstampPrice> _exchangeClient;
    private readonly IHttpClientFactory _httpClientFactory;

    public BitstampExchangeHandler(
        IExchangeHttpClient<TradeRequestParameter, BitstampPrice> exchangeClient, 
        IHttpClientFactory httpClientFactory)
    {
        _exchangeClient = exchangeClient;
        _httpClientFactory = httpClientFactory;
    }

    public string Name { get; } = "Bitstamp";

    public async Task<List<TradePrice>> Handle(ReadDataRequest request, DataSourceInfo dataSourceInfo)
    {
        using var client = _httpClientFactory.CreateClient();
        var price = await _exchangeClient.MakeCall(client, new TradeRequestParameter()
        {
            BaseUrl = dataSourceInfo.Url,
            Step = dataSourceInfo.Step,
            Start = request.Start,
            End = request.End,
            Candle = request.Candle
        });

        if (!price.Data.TradeInfo.Any())
            throw new ArgumentOutOfRangeException(nameof(price), "Exchange haven`t information for such criteria");

        var result = new List<TradePrice>(price.Data.TradeInfo.Count);

        foreach (var ohlc in price.Data.TradeInfo)
        {
            result.Add(new TradePrice()
            {
                TimeStamp = int.Parse(ohlc.Timestamp),
                Candle = request.Candle,
                Price = int.Parse(ohlc.Close)
            });
        }

        return result;
    }
}