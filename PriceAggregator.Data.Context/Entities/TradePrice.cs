using System.ComponentModel.DataAnnotations;

namespace PriceAggregator.Data.Context.Entities;

public class TradePrice : KeyEntity
{
    [Required]
    public decimal Price { get; set; }
    
    [Required]
    public int TimeStamp { get; set; }

    [Required]
    [MaxLength(50)]
    public string Candle { get; set; }
}