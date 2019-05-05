using Microsoft.EntityFrameworkCore;
using Coco.DAL.MappingConfigs.FarmMappings;
using Coco.Entities.Domain.Farm;

namespace Coco.DAL
{
    public class CocoDbContext : DbContext
    {
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
    }
}
