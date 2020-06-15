using Microsoft.EntityFrameworkCore;
using Coco.Entities.Domain.Identity;
using Coco.Contract;
using Coco.IdentityDAL.Mapping;
using Coco.Entities;

namespace Coco.IdentityDAL
{
    public class IdentityDbContext : CocoDbContext, IDbContext
    {
        #region DbSets
        public DbSet<User> User { get; set; }
        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<Gender> Gender { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<RoleClaim> RoleClaim { get; set; }
        public DbSet<Country> Country { get; set; }
        
        public DbSet<UserAttribute> UserAttribute { get; set; }
        public DbSet<AuthorizationPolicy> AuthorizationPolicy { get; set; }
        public DbSet<UserAuthorizationPolicy> UserAuthorizationPolicy { get; set; }
        public DbSet<RoleAuthorizationPolicy> RoleAuthorizationPolicy { get; set; }
        #endregion

        #region Ctor

        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options) { 
            
        }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserMap())
                .ApplyConfiguration(new UserInfoMap())
                .ApplyConfiguration(new RoleMap())
                .ApplyConfiguration(new UserRoleMap())
                .ApplyConfiguration(new RoleClaimMap())
                .ApplyConfiguration(new UserAttributeMap())
                .ApplyConfiguration(new AuthorizationPolicyMap())
                .ApplyConfiguration(new UserAuthorizationPolicyMap())
                .ApplyConfiguration(new RoleAuthorizationPolicyMap())
                .ApplyConfiguration(new StatusMap())
                .ApplyConfiguration(new CountryMap())
                .ApplyConfiguration(new GenderMap())
                .ApplyConfiguration(new UserClaimMap())
                .ApplyConfiguration(new UserTokenMap())
                .ApplyConfiguration(new UserLoginMap());
        }
    }
}
