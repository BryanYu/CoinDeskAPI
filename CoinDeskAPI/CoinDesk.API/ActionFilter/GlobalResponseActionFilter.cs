using System.Collections.Concurrent;
using System.Text.Json;
using CodeDesk.Service.Interfaces;
using CoinDesk.Model.Enum;
using CoinDesk.Model.Response;
using CoinDesk.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CoinDesk.API.ActionFilter;

public class GlobalResponseActionFilter : IResultFilter
{
    private readonly ILocalizeService _localizeService;

    public GlobalResponseActionFilter(ILocalizeService localizeService)
    {
        _localizeService = localizeService;
    }
    
    public void OnResultExecuting(ResultExecutingContext context)
    {
        var isExist = context.HttpContext.Items.TryGetValue("IsGenerateResponse", out var isGenerateResponse);
        if (isExist && isGenerateResponse.ToString() == bool.TrueString)
        {
            return;
        }

        if (context.Result is not ObjectResult)
        {
            return;
        }
        var resultObject = context.Result as ObjectResult;
        if (resultObject.Value is not HandlerResponse)
        {
            return;
        }
        var handlerResponse = resultObject.Value as HandlerResponse;
        var apiResponse = new ApiResponse<object>()
        {
            Status = handlerResponse.Status,
            Message = _localizeService.GetLocalizedString(LocalizeType.ApiResponseStatus,
                handlerResponse.Status.GetLocalizeKey()),
            Result = handlerResponse.Data
        };
        context.Result = new ObjectResult(apiResponse)
        {
            StatusCode = resultObject.StatusCode
        };       
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {
    }
}