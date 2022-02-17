﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Camino.Core.Contracts.Data
{
    public interface IAsyncRepository<TEntity> : IDisposable where TEntity : class
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
        /// Get first or default by filter async
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// Add entity
        /// </summary>
        /// <param name="entity">Entity</param>
        Task<TResult> AddAsync<TResult>(TEntity entity);

        /// <summary>
        /// Add entity async
        /// </summary>
        /// <param name="entities">entity</param>
        Task<object> AddAsync(TEntity entity);

        /// <summary>
        /// Add entities
        /// </summary>
        /// <param name="entities">Entities</param>
        Task AddAsync(IEnumerable<TEntity> entities);

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
        Task DeleteAsync(TEntity entity);

        /// <summary>
        /// Delete entities async
        /// </summary>
        /// <param name="entity">Entity</param>
        Task DeleteAsync(IQueryable<TEntity> entities);
    }
}
