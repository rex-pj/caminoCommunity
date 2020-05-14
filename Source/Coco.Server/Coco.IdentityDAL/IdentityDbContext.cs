using Microsoft.EntityFrameworkCore;
using Coco.Entities.Domain.Identity;
using Coco.Entities.Domain.Auth;
using Coco.Entities.Domain.Dbo;
using Coco.Entities.Base;
using System.Threading.Tasks;
using Coco.Contract;
using Coco.Entities.Domain.Work;

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
            // User
            modelBuilder.Entity<User>()
                .HasMany(x => x.CreatedUsers)
                .WithOne(x => x.CreatedBy)
                .HasForeignKey(x => x.CreatedById);

            modelBuilder.Entity<User>()
                .HasMany(x => x.UpdatedUsers)
                .WithOne(x => x.UpdatedBy)
                .HasForeignKey(x => x.UpdatedById);

            modelBuilder.Entity<User>()
                .HasOne(x => x.UserInfo)
                .WithOne(x => x.User)
                .HasForeignKey<UserInfo>(x => x.Id);

            modelBuilder.Entity<User>()
                .HasMany(c => c.UserCareers)
               .WithOne(x => x.User)
               .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<User>()
                .HasMany(c => c.UserRoles)
               .WithOne(x => x.User)
               .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<User>()
                .HasMany(c => c.UserAuthorizationPolicies)
                .WithOne(x => x.User)
                .HasForeignKey(c => c.UserId);

            // Role
            modelBuilder.Entity<Role>()
               .HasMany(c => c.UserRoles)
               .WithOne(x => x.Role)
               .HasForeignKey(c => c.RoleId);

            // UserRole
            modelBuilder.Entity<UserRole>()
               .HasKey(table => new
               {
                   table.UserId,
                   table.RoleId
               });

            // UserAuthorizationPolicy
            modelBuilder.Entity<UserAuthorizationPolicy>()
               .HasKey(table => new
               {
                   table.UserId,
                   table.AuthorizationPolicyId
               });

            // RoleAuthorizationPolicy
            modelBuilder.Entity<RoleAuthorizationPolicy>()
               .HasKey(table => new
               {
                   table.RoleId,
                   table.AuthorizationPolicyId
               });

            // UserCareer
            modelBuilder.Entity<UserCareer>()
               .HasKey(table => new
               {
                   table.UserId,
                   table.CareerId
               });

            base.OnModelCreating(modelBuilder);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
