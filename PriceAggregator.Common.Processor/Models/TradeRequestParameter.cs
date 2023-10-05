namespace PriceAggregator.Common.Processor.Models;

public class TradeRequestParameter : ReadDataRequest
{
    public int Step { get; set; } = 3600;
    public string BaseUrl { get; set; }

    public int Limit
    {
        get
        {
            var totalHours =  (int)(End - Start).TotalHours;
            return totalHours == 0 ? 1 : totalHours;
        }
    }
}