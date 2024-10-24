using System.ComponentModel.DataAnnotations;
using CoinDesk.Model.ModelBinder;
using Microsoft.AspNetCore.Mvc;

namespace CoinDesk.Model.Request;

public class PaginationRequest
{
    /// <summary>
    /// 每頁幾筆
    /// </summary>
    [Required]
    [ModelBinder(BinderType = typeof(PageSizeModelBinder))]
    public int PageSize { get; set; }

    /// <summary>
    /// 第幾頁
    /// </summary>
    [Required]
    [ModelBinder(BinderType = typeof(PageNumberModelBinder))]
    public int PageNumber { get; set; }
}