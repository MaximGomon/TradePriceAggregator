using System.ComponentModel.DataAnnotations;

namespace PriceAggregator.Data.Context.Entities;

public class DataSources : KeyEntity
{
    [Required]
    public int Number { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    [Required]
    [MaxLength(100)]
    public string HandlerName { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Url { get; set; }
}