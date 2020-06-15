using Microsoft.EntityFrameworkCore;
using Coco.Entities.Domain.Content;
using Coco.Contract;
using Coco.DAL.Mapping;
using Coco.Entities;

namespace Coco.DAL
{
    public class ContentDbContext : CocoDbContext, IDbContext
    {
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
    }
}
