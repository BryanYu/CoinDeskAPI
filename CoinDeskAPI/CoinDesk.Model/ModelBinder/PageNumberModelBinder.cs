using CoinDesk.Model.Request;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CoinDesk.Model.ModelBinder;

public class PageNumberModelBinder : IModelBinder
{
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
        var pageNumberValue = valueProviderResult.FirstValue;

        if (string.IsNullOrEmpty(pageNumberValue) || !int.TryParse(pageNumberValue, out var parsePageNumber))
        {
            bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, nameof(PaginationRequest.PageNumber));
            return Task.CompletedTask;
        }
        var pageNumber = parsePageNumber;

        if (parsePageNumber <= 0)
        {
            pageNumber = 1;
        }

        bindingContext.Result = ModelBindingResult.Success(pageNumber);
        return Task.CompletedTask;
    }
}