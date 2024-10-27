using System.ComponentModel.DataAnnotations;
using CoinDesk.Model.Config;
using Microsoft.Extensions.Options;

namespace CoinDesk.Model.Request;

public class PaginationRequest : IValidatableObject
{
    /// <summary>
    /// 每頁幾筆
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "PageSizeMustBeGreaterThanZero")]
    public int PageSize { get; set; }

    /// <summary>
    /// 第幾頁
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "PageNumberMustBeGreaterThanZero")]
    public int PageNumber { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var paginationConfig = validationContext.GetService(typeof(IOptions<PaginationConfig>)) as IOptions<PaginationConfig>;

        if (PageSize > paginationConfig.Value.PageSize)
        {
            yield return new ValidationResult("PageSizeCantOverSetting", new[] { "PageSize" });
        }
    } 
}