using CoinDesk.Infrastructure.Repository.Interfaces;

namespace CoinDesk.Infrastructure.Repository.Base;

public class UnitOfWork : IUnitOfWork
{
    private readonly CurrencyDbContext _context;
    public ICurrencyRepository CurrencyRepository { get; }
    
    public UnitOfWork(CurrencyDbContext context, ICurrencyRepository currencyRepository)
    {
        _context = context;
        CurrencyRepository = currencyRepository;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}