using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading.Tasks;

namespace Coco.Contract
{
    public interface IDbContext
    {
        #region Methods

        /// <summary>
        /// Creates a DbSet that can be used to query and save instances of entity
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>A set for the given entity type</returns>
        DbSet<TEntity> Set<TEntity>() where TEntity :  class;

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

        //
        // Summary:
        // Provides access to database related information and operations for this context.
        DatabaseFacade Database { get; }
        #endregion
    }
}
