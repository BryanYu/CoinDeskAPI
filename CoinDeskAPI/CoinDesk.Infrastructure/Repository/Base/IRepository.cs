using System.Linq.Expressions;

namespace CoinDesk.Infrastructure.Repository.Base;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(object id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
    Task AddAsync(TEntity entity);
    Task DeleteAsync(object id);

    Task DeleteAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
}