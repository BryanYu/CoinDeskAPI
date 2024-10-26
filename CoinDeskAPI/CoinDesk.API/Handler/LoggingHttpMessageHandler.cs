namespace CoinDesk.API.Handler;

public class LoggingHttpMessageHandler : DelegatingHandler
{
    private readonly ILogger<LoggingHttpMessageHandler> _logger;

    public LoggingHttpMessageHandler(ILogger<LoggingHttpMessageHandler> logger)
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var responseMessage = await base.SendAsync(request, cancellationToken);

        var requestBody = string.Empty;
        if (request.Content != null)
        {
            requestBody = await request.Content.ReadAsStringAsync();
        }
        var responseBody = string.Empty;
        if (responseMessage.Content != null)
        {
            responseBody = await responseMessage.Content.ReadAsStringAsync();
        }        
        _logger.LogDebug("RequestBody:{RequestBody} ResponseBody:{ResponseBody}", requestBody, responseBody);
        return responseMessage;
    }
}

