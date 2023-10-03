using System.ComponentModel.DataAnnotations;

namespace PriceAggregator.Data.Context.Entities;

public class TradePrice : KeyEntity
{
    [Required]
    public decimal Price { get; set; }
}