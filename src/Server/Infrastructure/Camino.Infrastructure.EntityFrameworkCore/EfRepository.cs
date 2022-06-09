using Camino.Core.Domains;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Camino.Infrastructure.EntityFrameworkCore
{
    public class EfRepository<TEntity> : IEntityRepository<TEntity>, IRepository<TEntity>, IAsyncRepository<TEntity>, IDisposable where TEntity : class
    {
        #region Fields

        private readonly CaminoDbContext _dbContext;

        private DbSet<TEntity> _dbSet;

        /// <summary>
        /// Gets an entity set
        /// </summary>
        protected virtual DbSet<TEntity> DbSet
        {
            get
            {
                if (_dbSet == null)
                {
                    _dbSet = _dbContext.Set<TEntity>();
                }

                return _dbSet;
            }
        }
        #endregion

        #region Ctor

        public EfRepository(CaminoDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets a table
        /// </summary>
        public virtual IQueryable<TEntity> Table => DbSet;

        /// <summary>
        /// Get entities
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> Get()
        {
            return DbSet;
        }

        /// <summary>
        /// Get entities by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter)
        {
            return DbSet.Where(filter);
        }

        /// <summary>
        /// Filter the entities and select by selector
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="filter"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public IQueryable<TResult> Get<TResult>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TResult>> selector)
        {
            return DbSet.Where(filter).Select(selector);
        }

        /// <summary>
        /// Get entities async
        /// </summary>
        /// <returns></returns>
        public async Task<IList<TEntity>> GetAsync()
        {
            return await DbSet.ToListAsync();
        }

        /// <summary>
        /// Filter the entities and select by selector
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="filter"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public async Task<IList<TResult>> GetAsync<TResult>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TResult>> selector)
        {
            return await DbSet.Where(filter).Select(selector).ToListAsync();
        }

        /// <summary>
        /// Get entities by filter async
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<IList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await DbSet.Where(filter).ToListAsync();
        }

        /// <summary>
        /// Get first or default by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public TEntity Find(Expression<Func<TEntity, bool>> filter)
        {
            return DbSet.FirstOrDefault(filter);
        }

        /// <summary>
        /// Get first or default by filter async
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await DbSet.FirstOrDefaultAsync(filter);
        }

        /// <summary>
        /// Add entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Insert(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            DbSet.Add(entity);
        }

        /// <summary>
        /// Add entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void Insert(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            DbSet.AddRange(entities);
        }

        /// <summary>
        /// Add entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual async Task InsertAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await DbSet.AddAsync(entity);
        }

        /// <summary>
        /// Add entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual async Task InsertAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            await DbSet.AddRangeAsync(entities);
        }

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbContext.Update(entity);
        }

        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void Update(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            foreach (var entity in entities)
            {
                Update(entity);
            }
        }

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual async Task UpdateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await Task.Run(() => DbSet.Update(entity));
        }

        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual async Task UpdateAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            await Task.Run(() => DbSet.UpdateRange(entities));
        }

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual int Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            // TODO: test
            DbSet.Remove(entity);
            var modifiedCount = _dbContext.ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted).Count();
            return modifiedCount;
        }

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual int Delete(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            DbSet.RemoveRange(entities);
            var modifiedCount = _dbContext.ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted).Count();
            return modifiedCount;
        }

        /// <summary>
        /// Delete entities by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Number of deleted records.</returns>
        public virtual int Delete(Expression<Func<TEntity, bool>> filter)
        {
            return Delete(Get(filter));
        }

        /// <summary>
        /// Delete entity async
        /// </summary>
        /// <param name="entity">Entity</param>
        public async Task<int> DeleteAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            DbSet.Remove(entity);
            var modifiedCount = _dbContext.ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted).Count();
            return await Task.FromResult(modifiedCount);
        }

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual async Task<int> DeleteAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            DbSet.RemoveRange(entities);
            var modifiedCount = _dbContext.ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted).Count();
            return await Task.FromResult(modifiedCount);
        }


        /// <summary>
        /// Delete entities by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Number of deleted records.</returns>
        public virtual async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await DeleteAsync(Get(filter));
        }
        #endregion

        #region Disposable
        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
