using CodeDesk.Service.Implements;
using CodeDesk.Service.Interfaces;

namespace CoinDesk.API.Extension;

public static class ServiceExtension
{
    public static IServiceCollection AddCustomService(this IServiceCollection services)
    {
        services.AddScoped<ICurrencyService, CoinDeskService>();
        services.AddSingleton<ILocalizeService, LocalizeService>();

        return services;
    }
}