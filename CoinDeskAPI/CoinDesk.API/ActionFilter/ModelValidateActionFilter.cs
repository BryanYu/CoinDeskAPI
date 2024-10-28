using CodeDesk.Service.Interfaces;
using CoinDesk.Model.Config;
using CoinDesk.Model.Enum;
using CoinDesk.Model.Response;
using CoinDesk.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;

namespace CoinDesk.API.ActionFilter;

public class ModelValidateActionFilter : IActionFilter
{
    private readonly ILocalizeService _localizeService;
    private readonly IOptions<PaginationConfig> _options;

    public ModelValidateActionFilter(ILocalizeService localizeService, IOptions<PaginationConfig> options)
    {
        _localizeService = localizeService;
        _options = options;
    }
    
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Where(e => e.Value.Errors.Count > 0)
                .SelectMany(e => e.Value.Errors.Select(error =>
                     GenerateErrorInfo(e.Key, error.ErrorMessage))
                    .ToList()).ToList();

            var localizeKey = ApiResponseStatus.ModelValidError.GetLocalizeKey();
            var response = new BadRequestObjectResult(new ApiResponse<object>
            {
                Errors = errors,
                Result = null,
                Status = ApiResponseStatus.ModelValidError,
                Message = _localizeService.GetLocalizedString(LocalizeType.ApiResponseStatus, localizeKey)
            });
            context.Result = response;
            context.HttpContext.Items["IsGenerateResponse"] = true;
        }
    }

    private ErrorInfo GenerateErrorInfo(string key, string message)
    {
        var localizeMessage = _localizeService.GetLocalizedString(LocalizeType.ModelValidError, message);
        if (message == "PageSizeCantOverSetting")
        {
            return new ErrorInfo
            {
                Field = key,
                Message = string.Format(localizeMessage, _options.Value.PageSize)
            };
        }
        return new ErrorInfo
        {
            Field = key,
            Message = localizeMessage
        };
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        
    }
}