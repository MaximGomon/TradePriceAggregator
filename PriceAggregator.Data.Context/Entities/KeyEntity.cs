using System.ComponentModel.DataAnnotations;

namespace PriceAggregator.Data.Context.Entities;

public class KeyEntity
{
    [Key]
    public int Id { get; set; }
}