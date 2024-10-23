using CoinDesk.Infrastructure.Model;
using Microsoft.EntityFrameworkCore;

namespace CoinDesk.Infrastructure;

public class CurrencyDbContext : DbContext
{
    public CurrencyDbContext(DbContextOptions<CurrencyDbContext> options) : base(options)
    {
    }
    
    public DbSet<Currency> Currency { get; set; }
}