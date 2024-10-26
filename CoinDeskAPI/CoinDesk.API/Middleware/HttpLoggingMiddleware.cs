using Serilog;
namespace CoinDesk.API.Middleware;

public class HttpLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IDiagnosticContext _diagnosticContext;

    public HttpLoggingMiddleware(RequestDelegate next, IDiagnosticContext diagnosticContext)
    {
        _next = next;
        _diagnosticContext = diagnosticContext;
    }
    public async Task Invoke(HttpContext context)
    {
        context.Request.Headers.TryGetValue("RequestId", out var requestId);
        var queryString = context.Request.QueryString;
        var requestBodyStream = new MemoryStream();
        await context.Request.Body.CopyToAsync(requestBodyStream);
        requestBodyStream.Seek(0, SeekOrigin.Begin);

        using var requestStreamReader = new StreamReader(requestBodyStream);
        var requestBodyText = await requestStreamReader.ReadToEndAsync();

        requestBodyStream.Seek(0, SeekOrigin.Begin);
        context.Request.Body = requestBodyStream;

        var originalResponseBody = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        await _next(context);

        context.Response.Headers.TryAdd("RequestId", requestId);
        responseBody.Seek(0, SeekOrigin.Begin);
        using var responseStreamReader = new StreamReader(responseBody);
        var responseBodyText = await responseStreamReader.ReadToEndAsync();
        
        responseBody.Seek(0, SeekOrigin.Begin);
        await responseBody.CopyToAsync(originalResponseBody);

        _diagnosticContext.Set("QueryString", queryString);
        _diagnosticContext.Set("RequestId", requestId.ToString());
        _diagnosticContext.Set("RequestBody", requestBodyText);
        _diagnosticContext.Set("ResponseBody", responseBodyText);

    }
}