using PriceAggregator.Common.Processor.Contracts;
using PriceAggregator.Common.Processor.Models;
using PriceAggregator.Data.Context.Entities;

namespace PriceAggregator.Common.Processor.Handlers;

public class BitfinexExchangeHandler : IExchangeHandler
{
    private readonly IExchangeHttpClient<TradeRequestParameter, List<BitfinexPrice>> _exchangeClient;
    private readonly IHttpClientFactory _httpClientFactory;

    public BitfinexExchangeHandler(
        IExchangeHttpClient<TradeRequestParameter, List<BitfinexPrice>> exchangeClient,
        IHttpClientFactory httpClientFactory)
    {
        _exchangeClient = exchangeClient;
        _httpClientFactory = httpClientFactory;
    }

    public string Name { get; } = "Bitfinex";

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

        return price.Select(x => new TradePrice()
            {
                Candle = request.Candle,
                Price = x.ClosePrice,
                TimeStamp= x.Timestamp
            })
            .ToList();
    }
}