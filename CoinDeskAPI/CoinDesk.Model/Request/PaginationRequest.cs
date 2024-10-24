using System.ComponentModel.DataAnnotations;
using CoinDesk.Model.Config;
using Microsoft.Extensions.Options;

namespace CoinDesk.Model.Request;

public class PaginationRequest : IValidatableObject
{
    /// <summary>
    /// 每頁幾筆
    /// </summary>
    [Required]
    public int PageSize { get; set; }

    /// <summary>
    /// 第幾頁
    /// </summary>
    [Required]
    public int PageNumber { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var paginationConfig = validationContext.GetService(typeof(IOptions<PaginationConfig>)) as IOptions<PaginationConfig>;

        if (PageSize > paginationConfig.Value.PageSize)
        {
            yield return new ValidationResult("PageSize不可大於100", new[] { "PageSize" });
        }

        if (PageNumber < 1)
        {
            yield return new ValidationResult("PageNumber不可小於1", new[] { "PageNumber" });
        }
    }
}