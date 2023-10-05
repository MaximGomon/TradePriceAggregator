namespace PriceAggregator.Common.Processor.Models;

public class ReadDataRequest
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string Candle { get; set; } = "BTCUSD";
}