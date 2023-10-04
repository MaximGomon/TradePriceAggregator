using System.Text.Json.Serialization;

namespace TradePriceAggregator.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CandleType
{
    BTCUSD,
}