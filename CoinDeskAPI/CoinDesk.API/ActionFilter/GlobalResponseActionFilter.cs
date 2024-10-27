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
        if (context.Result is ObjectResult objectResult && objectResult.StatusCode >= 200 && objectResult.StatusCode < 300)
        {
            var apiResponse = new ApiResponse<object>
            {
                Status = ApiResponseStatus.Success,
                Message = _localizeService.GetLocalizedString(LocalizeType.ApiResponseStatus,
                    ApiResponseStatus.Success.GetLocalizeKey()),
                Result = objectResult.Value,
            };
            context.Result = new ObjectResult(apiResponse)
            {
                StatusCode = objectResult.StatusCode
            };
        }
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {
    }
}