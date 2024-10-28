using CoinDesk.Model.Response;
using MediatR;

namespace CoinDesk.Model.Query;

public class GetCurrencyQuery : IRequest<HandlerResponse> 
{
    /// <summary>
    /// 每頁幾筆
    /// </summary>
    public int PageSize { get; set; }
    
    /// <summary>
    /// 第幾頁
    /// </summary>
    public int PageNumber { get; set; }
}