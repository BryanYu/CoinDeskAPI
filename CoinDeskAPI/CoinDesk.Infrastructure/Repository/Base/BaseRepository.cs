﻿using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace CoinDesk.Infrastructure.Repository.Base;

public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly CurrencyDbContext _context;
    private DbSet<TEntity> _dbSet;

    public BaseRepository(CurrencyDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public async Task<TEntity?> GetByIdAsync(object id)
    {
        return await this._dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await this._dbSet.ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
    {
        IQueryable<TEntity> query = _dbSet;
        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (orderBy != null)
        {
            return await orderBy(query).ToListAsync();
        }
        return await query.ToListAsync();
    }

    public Task AddAsync(TEntity entity)
    {
        _dbSet.AddAsync(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(TEntity entityToDelete)
    {
        if (_context.Entry(entityToDelete).State == EntityState.Detached)
        {
            _dbSet.Attach(entityToDelete);
        }
        _dbSet.Remove(entityToDelete);
        return Task.CompletedTask;
    }
    
    public Task DeleteAsync(object id)
    {
        TEntity entity = this._dbSet.Find(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
        }
        return Task.CompletedTask;
    }

    public Task UpdateAsync(TEntity entityToUpdate)
    {
        _dbSet.Attach(entityToUpdate);
        this._context.Entry(entityToUpdate).State = EntityState.Modified;
        return Task.CompletedTask;
    }
}