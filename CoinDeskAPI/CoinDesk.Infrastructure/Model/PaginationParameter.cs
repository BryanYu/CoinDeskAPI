namespace CoinDesk.Infrastructure.Model;

public class PaginationParameter
{
    /// <summary>
    /// 每頁顯示幾筆
    /// </summary>
    public int PageSize { get; set; }
    
    /// <summary>
    /// 第幾頁
    /// </summary>
    public int PageNumber { get; set; }
}