using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Coco.Contract
{
    public interface IRepository<TEntity> where TEntity : class
    {
        #region Methods
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
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        TEntity Find(object id);

        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        Task<TEntity> FindAsync(object id);

        /// <summary>
        /// Add entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Add(TEntity entity);

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
        /// Delete by id
        /// </summary>
        /// <param name="entity">Entity</param>
        void Delete(int id);

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void Delete(IEnumerable<TEntity> entities);

        /// <summary>
        /// Update entity by Property Name
        /// </summary>
        /// <param name="entity">Entity</param>
        void UpdateByName(TEntity entity, object value, string propertyName, bool isIgnoreCase = false);

        /// <summary>
        /// Gets a table
        /// </summary>
        IQueryable<TEntity> Table { get; }
        #endregion
    }
}
