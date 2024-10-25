namespace CoinDesk.Infrastructure.Model;

public class PagedQueryResult<TEntity>
{
    public IEnumerable<TEntity> Items { get; set; }
    public int TotalRecords { get; set; }
}