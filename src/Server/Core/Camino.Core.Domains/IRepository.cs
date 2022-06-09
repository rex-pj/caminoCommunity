using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Camino.Core.Domains
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
        /// Filter the entities and select by selector
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="filter"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        IQueryable<TResult> Get<TResult>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TResult>> selector);

        /// <summary>
        /// Get first or default by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        TEntity Find(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// Add entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Insert(TEntity entity);

        /// <summary>
        /// Add entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void Insert(IEnumerable<TEntity> entities);

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
        int Delete(TEntity entity);

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        /// <returns>Number of deleted records.</returns>
        int Delete(IEnumerable<TEntity> entities);

        /// <summary>
        /// Delete entities by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        int Delete(Expression<Func<TEntity, bool>> filter);
    }
}
