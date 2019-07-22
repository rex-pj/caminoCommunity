using Microsoft.EntityFrameworkCore;
using Coco.DAL.MappingConfigs.FarmMappings;
using Coco.Entities.Domain.Farm;
using Coco.Contract;
using System.Threading.Tasks;
using Coco.Entities.Base;

namespace Coco.DAL
{
    public class CocoDbContext : DbContext, IDbContext
    {
        /// <summary>
        /// Creates a DbSet that can be used to query and save instances of entity
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>A set for the given entity type</returns>
        public virtual new DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }

        #region DbSets
        public DbSet<Product> Product { get; set; }
        #endregion

        #region Ctor

        public CocoDbContext(DbContextOptions<CocoDbContext> options) : base(options) { }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ProductMappingConfig());
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
