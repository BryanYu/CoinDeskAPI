using System.Linq.Expressions;
using CoinDesk.Infrastructure.Model;

namespace CoinDesk.Infrastructure.Repository.Base;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(object id);
    Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
    Task<PagedQueryResult<TEntity>> GetPagingAsync(Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        PaginationParameter pagingParameter = null);
    
    Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate);

    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
    Task AddAsync(TEntity entity);
    Task DeleteAsync(object id);

    Task DeleteAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
}