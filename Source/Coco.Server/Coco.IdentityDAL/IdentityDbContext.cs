using Microsoft.EntityFrameworkCore;
using Coco.Entities.Domain.Identity;
using Coco.Entities.Domain.Auth;
using System.Threading.Tasks;
using Coco.Contract;
using Coco.IdentityDAL.Mapping;

namespace Coco.IdentityDAL
{
    public class IdentityDbContext : DbContext, IDbContext
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
        public DbSet<User> User { get; set; }
        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<Gender> Gender { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<UserPhoto> UserPhoto { get; set; }
        public DbSet<UserPhotoType> UserPhotoType { get; set; }
        public DbSet<UserAttribute> UserAttribute { get; set; }
        public DbSet<AuthorizationPolicy> AuthorizationPolicy { get; set; }
        public DbSet<UserAuthorizationPolicy> UserAuthorizationPolicy { get; set; }
        public DbSet<RoleAuthorizationPolicy> RoleAuthorizationPolicy { get; set; }
        #endregion

        #region Ctor

        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options) { }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserMap())
                .ApplyConfiguration(new UserInfoMap())
                .ApplyConfiguration(new RoleMap())
                .ApplyConfiguration(new UserRoleMap())
                .ApplyConfiguration(new UserAttributeMap())
                .ApplyConfiguration(new AuthorizationPolicyMap())
                .ApplyConfiguration(new UserAuthorizationPolicyMap())
                .ApplyConfiguration(new RoleAuthorizationPolicyMap())
                .ApplyConfiguration(new StatusMap())
                .ApplyConfiguration(new CountryMap())
                .ApplyConfiguration(new UserPhotoMap())
                .ApplyConfiguration(new UserPhotoTypeMap())
                .ApplyConfiguration(new GenderMap())
                .ApplyConfiguration(new UserClaimMap())
                .ApplyConfiguration(new UserTokenMap())
                .ApplyConfiguration(new UserLoginMap());
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
