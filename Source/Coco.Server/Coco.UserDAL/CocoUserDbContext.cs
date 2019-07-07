using Microsoft.EntityFrameworkCore;
using Coco.IdentityDAL.MappingConfigs;
using Coco.Entities.Domain.Identity;
using Coco.Entities.Domain.Auth;
using Coco.Entities.Domain.Dbo;

namespace Coco.IdentityDAL
{
    public class CocoUserDbContext : DbContext
    {
        #region DbSets
        public DbSet<Gender> Gender { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Country> Country { get; set; }
        #endregion

        #region Ctor

        public CocoUserDbContext(DbContextOptions<CocoUserDbContext> options) : base(options) { }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new GenderMappingConfig());
            modelBuilder.ApplyConfiguration(new StatusMappingConfig());
            modelBuilder.ApplyConfiguration(new UserInfoMappingConfig());
            modelBuilder.ApplyConfiguration(new UserMappingConfig());

            modelBuilder.ApplyConfiguration(new RoleMappingConfig());
            modelBuilder.ApplyConfiguration(new UserRoleMappingConfig());

            modelBuilder.ApplyConfiguration(new CountryMappingConfig());

            modelBuilder.ApplyConfiguration(new CareerMappingConfig());
            modelBuilder.ApplyConfiguration(new UserCareerMappingConfig());
        }
    }
}
