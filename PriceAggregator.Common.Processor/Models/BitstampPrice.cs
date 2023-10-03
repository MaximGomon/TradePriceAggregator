using System.Text.Json.Serialization;

namespace PriceAggregator.Common.Processor.Models;

public class BitstampPrice
{
    [JsonPropertyName("data")]
    public Data Data { get; set; }
}

public class Data
{
    [JsonPropertyName("ohlc")]
    public List<Ohlc> TradeInfo { get; set; }

    [JsonPropertyName("pair")]
    public string Pair { get; set; }
}

public class Ohlc
{
    [JsonPropertyName("close")]
    public string Close { get; set; }

    [JsonPropertyName("timestamp")]
    public string Timestamp { get; set; }
}