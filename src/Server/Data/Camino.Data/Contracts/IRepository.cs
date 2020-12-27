using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Camino.Data.Contracts
{
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Gets a table
        /// </summary>
        IQueryable<TEntity> Table { get; }

        /// <summary>
        /// Get entities
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> Get();

        /// <summary>
        /// Get entities by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// Get entities async
        /// </summary>
        /// <returns></returns>
        Task<IList<TEntity>> GetAsync();

        /// <summary>
        /// Get entities by filter async
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<IList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// Get first or default
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        TEntity FirstOrDefault();

        /// <summary>
        /// Get first or default by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// Get first or default async
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<TEntity> FirstOrDefaultAsync();

        /// <summary>
        /// Get first or default by filter async
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// Add entity
        /// </summary>
        /// <param name="entity">Entity</param>
        object Add(TEntity entity);

        /// <summary>
        /// Add entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void Add(IEnumerable<TEntity> entities);

        /// <summary>
        /// Add with int32 entity
        /// </summary>
        /// <param name="entity">Entity</param>
        int AddWithInt32Entity(TEntity entity);

        /// <summary>
        /// Add with int32 entity
        /// </summary>
        /// <param name="entity">Entity</param>
        Task<int> AddWithInt32EntityAsync(TEntity entity);

        /// <summary>
        /// Add entity async
        /// </summary>
        /// <param name="entities">entity</param>
        Task<object> AddAsync(TEntity entity);

        /// <summary>
        /// Add with int64 entity
        /// </summary>
        /// <param name="entity">Entity</param>
        long AddWithInt64Entity(TEntity entity);

        /// <summary>
        /// Add with int64 entity
        /// </summary>
        /// <param name="entity">Entity</param>
        Task<long> AddWithInt64EntityAsync(TEntity entity);

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Update(TEntity entity);

        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void Update(IEnumerable<TEntity> entities);

        /// <summary>
        /// Update entity async
        /// </summary>
        /// <param name="entity">Entity</param>
        Task UpdateAsync(TEntity entity);

        /// <summary>
        /// Update entities async
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task UpdateAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Delete(TEntity entity);

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void Delete(IQueryable<TEntity> entities);

        /// <summary>
        /// Delete entity async
        /// </summary>
        /// <param name="entity">Entity</param>
        Task DeleteAsync(TEntity entity);

        /// <summary>
        /// Delete entities async
        /// </summary>
        /// <param name="entity">Entity</param>
        Task DeleteAsync(IQueryable<TEntity> entities);
    }
}
