namespace CoinDesk.Model.Response;

public class PagedResultResponse<T>
{
    public Pagination Pagination { get; set; }
    
    public IEnumerable<T> Items { get; set; }
}

public class Pagination
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalRecords { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);
}

