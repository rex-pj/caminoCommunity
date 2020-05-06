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

            // UserInfo
            modelBuilder.Entity<UserInfo>()
               .HasMany(c => c.UserPhotos)
               .WithOne(x => x.UserInfo)
               .HasForeignKey(c => c.UserId);

            // Role
            modelBuilder.Entity<Role>()
               .HasMany(c => c.UserRoles)
               .WithOne(x => x.Role)
               .HasForeignKey(c => c.RoleId);

            // Status
            modelBuilder.Entity<Status>()
                .HasMany(x => x.Users)
                .WithOne(x => x.Status)
                .HasForeignKey(x => x.StatusId);

            // UserRole
            modelBuilder.Entity<UserRole>()
               .HasKey(table => new
               {
                   table.UserId,
                   table.RoleId
               });

            // UserCareer
            modelBuilder.Entity<UserCareer>()
               .HasKey(table => new
               {
                   table.UserId,
                   table.CareerId
               });

            // Career
            modelBuilder.Entity<Career>()
              .HasMany(c => c.UserCareers)
              .WithOne(x => x.Career)
              .HasForeignKey(c => c.CareerId);

            // Country
            modelBuilder.Entity<Country>()
                .HasMany(x => x.UserInfos)
                .WithOne(x => x.Country)
                .HasForeignKey(x => x.CountryId);

            // Gender
            modelBuilder.Entity<Gender>()
                .HasMany(x => x.UserInfos)
                .WithOne(x => x.Gender)
                .HasForeignKey(x => x.GenderId);

            // UserPhoto
            modelBuilder.Entity<UserPhoto>()
                .HasOne(x => x.UserInfo)
                .WithMany(x => x.UserPhotos);

            base.OnModelCreating(modelBuilder);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
