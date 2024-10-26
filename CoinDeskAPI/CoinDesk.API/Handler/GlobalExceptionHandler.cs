using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CoinDesk.API.Handler;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IHostEnvironment _hostEnvironment;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IHostEnvironment hostEnvironment)
    {
        _logger = logger;
        _hostEnvironment = hostEnvironment;
    }
    
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Request.Headers.TryGetValue("RequestId", out var requestId);
        _logger.LogError(
            exception, "Exception occurred: {Message} RequestId:{RequestId}", exception.Message, requestId);

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        httpContext.Request.ContentType = "application/json";

        var errorResponse = new
        {
            Status = "Error",
            Message = _hostEnvironment.IsDevelopment() ? exception.Message : "Internal Server Error"
        };
        httpContext.Request.Headers.TryAdd("RequestId", requestId);
        await httpContext.Response
            .WriteAsJsonAsync(errorResponse, cancellationToken);

        return true;
    }
}