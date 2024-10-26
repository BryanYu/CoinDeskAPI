using CoinDesk.API.Middleware;

namespace CoinDesk.API.Extension;

public static class RequestIdExtension
{
    public static IApplicationBuilder UseCustomRequestIdOnHeader(this IApplicationBuilder app)
    {
        return app.UseMiddleware<AddRequestIdOnHeaderMiddleware>();
    }
}