using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Transactions;

namespace Camino.Data.Contracts
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        #region Fields

        private readonly IBaseDataProvider _dbProvider;

        private ITable<TEntity> _entities;

        /// <summary>
        /// Gets an entity set
        /// </summary>
        protected virtual ITable<TEntity> Entities
        {
            get
            {
                if (_entities == null)
                {
                    _entities = _dbProvider.GetTable<TEntity>();
                }

                return _entities;
            }
        }
        #endregion

        #region Ctor

        protected BaseRepository(IBaseDataProvider dbProvider)
        {
            _dbProvider = dbProvider;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets a table
        /// </summary>
        public virtual IQueryable<TEntity> Table => Entities;

        /// <summary>
        /// Get entities
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> Get()
        {
            return Entities;
        }

        /// <summary>
        /// Get entities by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter)
        {
            return Entities.Where(filter);
        }

        /// <summary>
        /// Get entities async
        /// </summary>
        /// <returns></returns>
        public async Task<IList<TEntity>> GetAsync()
        {
            return await Entities.ToListAsync();
        }

        /// <summary>
        /// Get entities by filter async
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<IList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await Entities.Where(filter).ToListAsync();
        }

        /// <summary>
        /// Get first or default
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public TEntity FirstOrDefault()
        {
            return Entities.FirstOrDefault();
        }

        /// <summary>
        /// Get first or default by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> filter)
        {
            return Entities.FirstOrDefault(filter);
        }

        /// <summary>
        /// Get first or default async
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<TEntity> FirstOrDefaultAsync()
        {
            return await Entities.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get first or default by filter async
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await Entities.FirstOrDefaultAsync(filter);
        }

        /// <summary>
        /// Add entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual object Add(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return _dbProvider.InsertWithIdentity(entity);
        }

        /// <summary>
        /// Add entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void Add(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            using (var transaction = new TransactionScope())
            {
                _dbProvider.InsertRange(entities);
                transaction.Complete();
            }
        }

        /// <summary>
        /// Add entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual async Task<object> AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return await _dbProvider.InsertWithIdentityAsync(entity);
        }

        /// <summary>
        /// Add with int64 entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual long AddWithInt64Entity(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return _dbProvider.InsertWithInt64Identity(entity);
        }

        /// <summary>
        /// Add with int64 entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual async Task<long> AddWithInt64EntityAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return await _dbProvider.InsertWithInt64IdentityAsync(entity);
        }

        /// <summary>
        /// Add with int32 entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual int AddWithInt32Entity(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return _dbProvider.InsertWithInt32Identity(entity);
        }

        /// <summary>
        /// Add with int32 entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual async Task<int> AddWithInt32EntityAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return await _dbProvider.InsertWithInt32IdentityAsync(entity);
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

            _dbProvider.Update(entity);
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

            await _dbProvider.UpdateAsync(entity);
        }

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbProvider.Delete(entity);
        }

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void Delete(IQueryable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            _dbProvider.DeleteRange(entities);
        }

        /// <summary>
        /// Delete entity async
        /// </summary>
        /// <param name="entity">Entity</param>
        public async Task DeleteAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await _dbProvider.DeleteAsync(entity);
        }

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual async Task DeleteAsync(IQueryable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            await _dbProvider.DeleteRangeAsync(entities);
        }
        #endregion
    }
}
