namespace PriceAggregator.Common.Processor.Models;

public class TradeRequestParameter
{
    public DateTime Time { get; set; }
    public string Candle { get; set; } = "BTCUSD";
    public int Step { get; set; } = 3600;
    public string BaseUrl { get; set; }
}