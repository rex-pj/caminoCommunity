using Microsoft.EntityFrameworkCore;
using Coco.Entities.Domain.Agri;
using Coco.Contract;
using System.Threading.Tasks;
using Coco.Entities.Base;
using Coco.Entities.Domain.Content;

namespace Coco.DAL
{
    public class ContentDbContext : DbContext, IDbContext
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

        public DbSet<ArticleCategory> ArticleCategory { get; set; }
        #endregion

        #region Ctor

        public ContentDbContext(DbContextOptions<ContentDbContext> options) : base(options) { }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
