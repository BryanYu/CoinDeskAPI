using CoinDesk.Model.Config;

namespace CoinDesk.API.Extension;

public static class ConfigureExtension
{
    public static IServiceCollection AddCustomConfigure(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.Configure<PaginationConfig>(configuration.GetSection("PaginationConfig"));
        services.Configure<CoinDeskConfig>(configuration.GetSection("CoinDeskConfig"));
        return services;
    }
}