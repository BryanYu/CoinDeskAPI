namespace CoinDesk.Model.Response;

public class PagedResultResponse<T>
{
    /// <summary>
    /// 分頁設定資訊
    /// </summary>
    public Pagination Pagination { get; set; }
    
    /// <summary>
    /// 分頁資料
    /// </summary>
    public T Data { get; set; }
}

public class Pagination
{
    /// <summary>
    /// 第幾頁
    /// </summary>
    public int PageNumber { get; set; }
    
    /// <summary>
    /// 一頁幾筆
    /// </summary>
    public int PageSize { get; set; }
    
    /// <summary>
    /// 總筆數
    /// </summary>
    public int TotalRecords { get; set; }
    
    /// <summary>
    /// 總頁數
    /// </summary>
    public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);
}

