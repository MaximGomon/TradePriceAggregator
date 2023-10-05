using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;
using PriceAggregator.Common.Extensions;
using PriceAggregator.Common.Processor.Contracts;
using PriceAggregator.Common.Processor.Models;

namespace PriceAggregator.Common.Processor.Clients;

public class BitfinexHttpClient : IExchangeHttpClient<TradeRequestParameter, List<BitfinexPrice>>
{
    private readonly ILogger<BitfinexHttpClient> _logger;

    public BitfinexHttpClient(ILogger<BitfinexHttpClient> logger)
    {
        _logger = logger;
    }

    public async Task<List<BitfinexPrice>> MakeCall(HttpClient client, TradeRequestParameter parameter)
    {
        var url =
            $"{parameter.BaseUrl}/trade:{parameter.Step.SecondToBitfinexTime()}:t{parameter.Candle.ToUpperInvariant()}" +
            $"/hist?start={parameter.Start.ToEpochTimeMilliseconds()}&end={parameter.End.ToEpochTimeMilliseconds()}" +
            $"&limit={parameter.Limit}";
        var response = await client.GetAsync(url);
        var responseText = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            var message = $"Can`t get Bitfinex trade details. Details: {responseText}";
            _logger.LogError(message);

            throw new ArgumentException(message);
        }

        var result = new List<BitfinexPrice>(parameter.Limit);
        var jsonObject = JsonObject.Parse(responseText);
        var root = jsonObject.Root.AsArray();

        foreach (var node in root)
        {
            var nodeArray = node.AsArray();
            var close = nodeArray[2].GetValue<int>();
            var timestamp = nodeArray[0].GetValue<double>().FromEpochMilliseconds().ToEpochTime();

            var price = new BitfinexPrice()
            {
                ClosePrice = close,
                Timestamp = timestamp
            };
            result.Add(price);
        }
        return result;
    }
}