using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using CodeDesk.Service.Interfaces;
using CoinDesk.Model.Enum;
using CoinDesk.Model.Response;
using CoinDesk.Utility;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CoinDesk.API.Handler;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IHostEnvironment _hostEnvironment;
    private readonly ILocalizeService _localizeService;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IHostEnvironment hostEnvironment, ILocalizeService localizeService)
    {
        _logger = logger;
        _hostEnvironment = hostEnvironment;
        _localizeService = localizeService;
    }
    
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Request.Headers.TryGetValue("RequestId", out var requestId);
        _logger.LogError(
            exception, "Exception occurred: {Message} RequestId:{RequestId}", exception.Message, requestId);

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        httpContext.Request.ContentType = "application/json";

        var message = _hostEnvironment.IsDevelopment()
            ? exception.Message
            : _localizeService.GetLocalizedString(LocalizeType.ApiResponseStatus,
                ApiResponseStatus.InternalServerError.GetLocalizeKey());

        var apiResponse = new ApiResponse<object>
        {
            Status = ApiResponseStatus.InternalServerError,
            Message = message,
        };
        httpContext.Request.Headers.TryAdd("RequestId", requestId);
        httpContext.Response.ContentType = "application/json";

        var jsonResult = JsonSerializer.Serialize(apiResponse, new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        await httpContext.Response
            .WriteAsync(jsonResult, cancellationToken);

        return true;
    }
}