using Microsoft.EntityFrameworkCore;
using Coco.Entities.Domain.Content;
using Coco.Contract;
using System.Threading.Tasks;
using Coco.DAL.Mapping;

namespace Coco.DAL
{
    public class ContentDbContext : DbContext, IDbContext
    {
        /// <summary>
        /// Creates a DbSet that can be used to query and save instances of entity
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>A set for the given entity type</returns>
        public virtual new DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        #region DbSets
        public DbSet<Product> Product { get; set; }

        public DbSet<ArticleCategory> ArticleCategory { get; set; }
        public DbSet<UserPhoto> UserPhoto { get; set; }
        public DbSet<UserPhotoType> UserPhotoType { get; set; }
        #endregion

        #region Ctor

        public ContentDbContext(DbContextOptions<ContentDbContext> options) : base(options) { }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfiguration(new ArticleCategoryMap())
                .ApplyConfiguration(new UserPhotoMap())
                .ApplyConfiguration(new UserPhotoTypeMap());

            base.OnModelCreating(modelBuilder);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
