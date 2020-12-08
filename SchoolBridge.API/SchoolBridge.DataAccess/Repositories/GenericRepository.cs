using SchoolBridge.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SchoolBridge.DataAccess.Repositories
{
    public class GenericRepository<TEntity>
        : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _db;

        public GenericRepository(DbContext db)
            => _db = db;

        public async Task<int> CountAllAsync() => 
            await _db.Set<TEntity>().CountAsync();
        public int CountAll() =>
           _db.Set<TEntity>().Count();

        public async Task<int> CountWhereAsync(Expression<Func<TEntity, bool>> predicate)
          => await _db.Set<TEntity>().CountAsync(predicate);

        public int CountWhere(Expression<Func<TEntity, bool>> predicate)
          => _db.Set<TEntity>().Count(predicate);

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            var entry = await _db.Set<TEntity>().AddAsync(entity);
            await _db.SaveChangesAsync();
            return entry.Entity;
        }

        public TEntity Create(TEntity entity)
        {
            var entry = _db.Set<TEntity>().Add(entity);
            _db.SaveChanges();
            return entry.Entity;
        }

        public IEnumerable<TEntity> Create(IEnumerable<TEntity> entities)
        {
            List<TEntity> added = new List<TEntity>();
            EntityEntry<TEntity> entry;

            foreach (var item in entities)
            {
                entry = _db.Set<TEntity>().Add(item);
                added.Add(entry.Entity);
            }
            _db.SaveChanges();

            return added;
        }


        public async Task<IEnumerable<TEntity>> CreateAsync(IEnumerable<TEntity> entities)
        {
            List<TEntity> added = new List<TEntity>();
            EntityEntry<TEntity> entry;

            foreach (var item in entities) {
                entry = await _db.Set<TEntity>().AddAsync(item);
                added.Add(entry.Entity);
            }
            await _db.SaveChangesAsync();

            return added;
        }

        public async Task DeleteAsync(TEntity entity)
        {
            _db.Set<TEntity>().Remove(entity);
            await _db.SaveChangesAsync();
        }

        public void Delete(TEntity entity)
        {
            _db.Set<TEntity>().Remove(entity);
            _db.SaveChanges();
        }

        public async Task DeleteAsync(IEnumerable<TEntity> entity)
        {
            _db.Set<TEntity>().RemoveRange(entity);
            await _db.SaveChangesAsync();
        }

        public void Delete(IEnumerable<TEntity> entity)
        {
            _db.Set<TEntity>().RemoveRange(entity);
            _db.SaveChanges();
        }

        public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            _db.Set<TEntity>().RemoveRange(predicate != null ? _db.Set<TEntity>().Where(predicate).AsEnumerable() : _db.Set<TEntity>().AsEnumerable());
            await _db.SaveChangesAsync();
        }

        public void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            
            _db.Set<TEntity>().RemoveRange(predicate != null ? _db.Set<TEntity>().Where(predicate).AsEnumerable() : _db.Set<TEntity>().AsEnumerable());
            _db.SaveChanges();
        }

        public async Task<TEntity> FindAsync<Type>(Type Id)
            => await _db.Set<TEntity>().FindAsync(Id);

        public TEntity Find<Type>(Type Id)
            => _db.Set<TEntity>().Find(Id);

        public async Task<IEnumerable<TEntity>> GetAllAsync()
            => await _db.Set<TEntity>().ToListAsync();

        public IEnumerable<TEntity> GetAll()
            => _db.Set<TEntity>().ToList();

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
            => await _db.Set<TEntity>().Where(predicate).ToListAsync();

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
            => _db.Set<TEntity>().Where(predicate).ToList();

        public async Task<IEnumerable<TEntity>> GetAllIncludeAsync(params Expression<Func<TEntity, object>>[] includes)
        {
            return await includes.Aggregate(_db.Set<TEntity>().AsQueryable(), 
                (current, includeProperty) =>
                    current.Include(includeProperty)).ToListAsync();
        }

        public IEnumerable<TEntity> GetAllInclude(params Expression<Func<TEntity, object>>[] includes)
        {
            return includes.Aggregate(_db.Set<TEntity>().AsQueryable(),
                (current, includeProperty) =>
                    current.Include(includeProperty)).ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAllIncludeAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            return await includes.Aggregate(_db.Set<TEntity>().Where(predicate),
                (current, includeProperty) =>
                    current.Include(includeProperty)).ToListAsync();
        }

        public IEnumerable<TEntity> GetAllInclude(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            return includes.Aggregate(_db.Set<TEntity>().Where(predicate),
                (current, includeProperty) =>
                    current.Include(includeProperty)).ToList();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _db.Entry(entity).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }
        public void Update(TEntity entity)
        {
            _db.Entry(entity).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public DbSet<TEntity> GetDbSet()
        {
            return _db.Set<TEntity>();
        }

        public DbContext GetDbContext()
        {
            return _db;
        }
    }
}