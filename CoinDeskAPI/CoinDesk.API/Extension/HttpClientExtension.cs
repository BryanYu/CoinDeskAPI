using CoinDesk.API.Handler;

namespace CoinDesk.API.Extension;

public static class HttpClientExtension
{
    public static IServiceCollection AddCustomHttpClient(this IServiceCollection services)
    {
        services.AddHttpClient("LoggingHttpClient")
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler())
            .AddHttpMessageHandler((serviceProvider) =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<LoggingHttpMessageHandler>>();
                return new LoggingHttpMessageHandler(logger);
            });
        return services;
    }
}