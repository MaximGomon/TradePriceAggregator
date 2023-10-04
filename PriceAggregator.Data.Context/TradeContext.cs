using Microsoft.EntityFrameworkCore;
using PriceAggregator.Data.Context.Entities;

namespace PriceAggregator.Data.Context
{
    public class TradeContext : DbContext
    {
        public TradeContext(DbContextOptions<TradeContext> options) : base(options)
        {
        }

        public DbSet<DataSourceInfo> DataSources { get; set; }
        public DbSet<TradePrice> TradePrices { get; set; }
    }
}