using Microsoft.EntityFrameworkCore;
using Coco.IdentityDAL.MappingConfigs;
using Coco.Entities.Domain.Identity;
using Coco.Entities.Domain.Auth;
using Coco.Entities.Domain.Dbo;
using Coco.Entities.Base;
using System.Threading.Tasks;
using Coco.Contract;

namespace Coco.IdentityDAL
{
    public class IdentityDbContext : DbContext, IDbContext
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
        public DbSet<Gender> Gender { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<UserPhoto> UserPhoto { get; set; }
        #endregion

        #region Ctor

        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options) { }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new GenderMappingConfig())
                .ApplyConfiguration(new StatusMappingConfig())
                .ApplyConfiguration(new UserPhotoMappingConfig())
                .ApplyConfiguration(new UserInfoMappingConfig())
                .ApplyConfiguration(new UserMappingConfig())
                .ApplyConfiguration(new UserAttributeMappingConfig())
                .ApplyConfiguration(new RoleMappingConfig())
                .ApplyConfiguration(new UserRoleMappingConfig())
                .ApplyConfiguration(new CountryMappingConfig())
                .ApplyConfiguration(new CareerMappingConfig())
                .ApplyConfiguration(new UserCareerMappingConfig());
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
