namespace CoinDesk.API.Extension;

public static class SwaagerGenExtension
{
    public static IServiceCollection AddCustomSwaagerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            var apiXmlPath = Path.Combine(AppContext.BaseDirectory, "CoinDesk.API.xml");
            var modelXmlPath = Path.Combine(AppContext.BaseDirectory, "CoinDesk.Model.xml");
            options.IncludeXmlComments(apiXmlPath);
            options.IncludeXmlComments(modelXmlPath);
        });
        return services;
    } 
}