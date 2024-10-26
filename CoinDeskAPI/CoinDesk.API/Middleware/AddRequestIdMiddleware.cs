namespace CoinDesk.API.Middleware;

public class AddRequestIdOnHeaderMiddleware
{
    private readonly RequestDelegate _next;
    
    public AddRequestIdOnHeaderMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var requestId = Guid.NewGuid().ToString();
        context.Request.Headers.TryAdd("RequestId", requestId);
        await _next(context);
        context.Response.Headers.TryAdd("RequestId", requestId);
    }
}