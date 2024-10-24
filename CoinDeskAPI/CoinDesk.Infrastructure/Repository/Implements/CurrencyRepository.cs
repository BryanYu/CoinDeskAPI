using CoinDesk.Infrastructure.Model;
using CoinDesk.Infrastructure.Repository.Base;
using CoinDesk.Infrastructure.Repository.Interfaces;

namespace CoinDesk.Infrastructure.Repository.Implements;

public class CurrencyRepository : BaseRepository<Currency>, ICurrencyRepository
{
    public CurrencyRepository(CurrencyDbContext context) : base(context)
    {
    }
}