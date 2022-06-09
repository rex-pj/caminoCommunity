using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Camino.Core.Domains
{
    public interface IAsyncRepository<TEntity> where TEntity : class
    {
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
        /// Filter the entities and select by selector
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="filter"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        Task<IList<TResult>> GetAsync<TResult>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TResult>> selector);
        /// <summary>
        /// Get first or default by filter async
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// Add entity
        /// </summary>
        /// <param name="entity">Entity</param>
        Task InsertAsync(TEntity entity);

        /// <summary>
        /// Add entities
        /// </summary>
        /// <param name="entities">Entities</param>
        Task InsertAsync(IEnumerable<TEntity> entities);

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
        /// Delete entity async
        /// </summary>
        /// <param name="entity">Entity</param>
        Task<int> DeleteAsync(TEntity entity);

        /// <summary>
        /// Delete entities async
        /// </summary>
        /// <param name="entity">Entity</param>
        Task<int> DeleteAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Delete entities by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Number of deleted records.</returns>
        Task<int> DeleteAsync(Expression<Func<TEntity, bool>> filter);
    }
}
