using Microsoft.EntityFrameworkCore;

namespace PriceAggregator.Data.Context
{
    public class TradeContext : DbContext
    {
        public TradeContext(DbContextOptions<TradeContext> options) : base(options)
        {
        }
    }
}