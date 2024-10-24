using CoinDesk.Model.Config;
using CoinDesk.Model.Request;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace CoinDesk.Model.ModelBinder;

public class PageSizeModelBinder : IModelBinder
{
    private readonly PaginationConfig _config;

    public PageSizeModelBinder(IOptions<PaginationConfig> options)
    {
        _config = options.Value;
    }
    
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }
        var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

        if (valueProviderResult == ValueProviderResult.None)
        {
            return Task.CompletedTask;
        }
        bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);
        var pageSizeValue = valueProviderResult.FirstValue;

        if (string.IsNullOrEmpty(pageSizeValue) || !int.TryParse(pageSizeValue, out var parsePageSize))
        {
            bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, nameof(PaginationRequest.PageSize));
            return Task.CompletedTask;
        }
        var pageSize = parsePageSize;
        if (parsePageSize >= _config.PageSize || parsePageSize <= 0)
        {
            pageSize = _config.PageSize;
        }
        bindingContext.Result = ModelBindingResult.Success(pageSize);
        return Task.CompletedTask;
    }
}