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

    public async Task<TradePrice> Handle(DateTime time, DataSourceInfo dataSourceInfo)
    {
        using var client = _httpClientFactory.CreateClient();
        var price = await _exchangeClient.MakeCall(client, new TradeRequestParameter()
        {
            BaseUrl = dataSourceInfo.Url,
            Step = dataSourceInfo.Step,
            Time = time
        });

        if (!price.Data.TradeInfo.Any())
            throw new ArgumentOutOfRangeException(nameof(price), "Exchange haven`t information for such criteria");

        if (int.TryParse(price.Data.TradeInfo.First().Close, out int parsedClose))
        {
            var result = new TradePrice()
            {
                Time = time,
                Price = (decimal) parsedClose
            };
            return result;
        }

        throw new ArgumentException($"Can`t deserialize Close value \'{price.Data.TradeInfo.First().Close}\'");
    }
}