using LinqToDB;
using System.Threading.Tasks;

namespace Coco.Contract
{
    public interface IDbConnection
    {
        #region Methods

        /// <summary>
        /// Creates a DbSet that can be used to query and save instances of entity
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>A set for the given entity type</returns>
        ITable<TEntity> Set<TEntity>() where TEntity :  class;

        /// <summary>
        /// Saves all changes made in this context to the database
        /// </summary>
        /// <returns>The number of state entries written to the database</returns>
        int SaveChanges();

        /// <summary>
        /// Saves all changes made in this context to the database
        /// </summary>
        /// <returns>The number of state entries written to the database</returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Update entity by Property Name
        /// </summary>
        /// <param name="entity">Entity</param>
        void UpdateByName<TEntity>(TEntity entity, object value, string propertyName, bool isIgnoreCase = false) where TEntity : class;
        #endregion
    }
}
