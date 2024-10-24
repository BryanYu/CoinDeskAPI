namespace CoinDesk.Infrastructure.Model;

public class PagedResult<TEntity>
{
    public IEnumerable<TEntity> Items { get; set; }
    public int TotalRecords { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);
}