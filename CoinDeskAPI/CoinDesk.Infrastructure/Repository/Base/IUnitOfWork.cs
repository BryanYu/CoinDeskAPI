using CoinDesk.Infrastructure.Repository.Interfaces;

namespace CoinDesk.Infrastructure.Repository.Base;

public interface IUnitOfWork
{
    ICurrencyRepository CurrencyRepository { get; }
    
    Task<int> SaveChangesAsync();
}