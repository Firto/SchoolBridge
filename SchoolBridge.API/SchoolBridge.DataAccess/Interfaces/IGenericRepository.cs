using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SchoolBridge.DataAccess.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        TEntity Create(TEntity entity);
        IEnumerable<TEntity> Create(IEnumerable<TEntity> entities);
        Task<TEntity> CreateAsync(TEntity entity);
        Task<IEnumerable<TEntity>> CreateAsync(IEnumerable<TEntity> entities);
        Task UpdateAsync(TEntity entity);
        void Update(TEntity entity);
        Task DeleteAsync(TEntity entity);
        void Delete(TEntity entity);
        Task DeleteAsync(IEnumerable<TEntity> entity);
        void Delete(IEnumerable<TEntity> entity);
        Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);
        void Delete(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetAllAsync();
        IEnumerable<TEntity> GetAll();
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetAllIncludeAsync(params Expression<Func<TEntity, object>>[] includes);
        IEnumerable<TEntity> GetAllInclude(params Expression<Func<TEntity, object>>[] includes);
        Task<IEnumerable<TEntity>> GetAllIncludeAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);
        IEnumerable<TEntity> GetAllInclude(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);
        Task<TEntity> FindAsync<Type>(Type Id);
        TEntity Find<Type>(Type Id);
        Task<int> CountAllAsync();
        int CountAll();
        Task<int> CountWhereAsync(Expression<Func<TEntity, bool>> predicate);
        int CountWhere(Expression<Func<TEntity, bool>> predicate);
        DbSet<TEntity> GetDbSet();
        DbContext GetDbContext();
    }
}