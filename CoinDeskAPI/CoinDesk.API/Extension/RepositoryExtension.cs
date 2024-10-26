using CoinDesk.Infrastructure.Repository.Base;
using CoinDesk.Infrastructure.Repository.Implements;
using CoinDesk.Infrastructure.Repository.Interfaces;

namespace CoinDesk.API.Extension;

public static class RepositoryExtension
{
    public static IServiceCollection AddCustomRepositoy(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        return services;
    }
}