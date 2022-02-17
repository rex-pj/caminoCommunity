using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Camino.Core.Contracts.Data
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
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
        /// Get first or default by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// Add entity
        /// </summary>
        /// <param name="entity">Entity</param>
        object Add(TEntity entity);

        /// <summary>
        /// Add entities
        /// </summary>
        /// <param name="entity">Entity</param>
        TResult Add<TResult>(TEntity entity);

        /// <summary>
        /// Add entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void Add(IEnumerable<TEntity> entities);

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
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Delete(TEntity entity);

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void Delete(IQueryable<TEntity> entities);
    }
}
