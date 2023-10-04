using System.Text.Json;
using Microsoft.Extensions.Logging;
using PriceAggregator.Common.Extensions;
using PriceAggregator.Common.Processor.Contracts;
using PriceAggregator.Common.Processor.Models;

namespace PriceAggregator.Common.Processor.Clients;

public class BitstampHttpClient : IExchangeHttpClient<TradeRequestParameter, BitstampPrice>
{
    private readonly ILogger<BitstampHttpClient> _logger;

    public BitstampHttpClient(ILogger<BitstampHttpClient> logger)
    {
        _logger = logger;
    }

    public async Task<BitstampPrice> MakeCall(HttpClient client, TradeRequestParameter parameter)
    {
        var response = await client.GetAsync($"{parameter.BaseUrl}/{parameter.Candle.ToLowerInvariant()}/?step={parameter.Step}&limit=1&start={parameter.Time.ToEpochTime()}");
        var responseText = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            var message = $"Can`t get Bitstamp trade details. Details: {responseText}";
            _logger.LogError(message);

            throw new ArgumentException(message);
        }

        var model = JsonSerializer.Deserialize<BitstampPrice>(responseText);
        return model;
    }
}