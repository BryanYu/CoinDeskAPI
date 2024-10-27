namespace CoinDesk.API.Extension;

public static class RequestLocalizationExtension
{
    public static IApplicationBuilder UseCustomRequestLocalization(this IApplicationBuilder app)
    {
        app.UseRequestLocalization(item =>
        {
            var supportedCultures = new[] { "zh-Hant", "en-US", "zh-Hans" };
            item.SetDefaultCulture(supportedCultures[0]).AddSupportedUICultures(supportedCultures);
        });
        return app;
    }
}