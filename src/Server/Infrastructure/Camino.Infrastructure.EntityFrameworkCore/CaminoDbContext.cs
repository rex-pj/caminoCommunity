using Camino.Infrastructure.EntityFrameworkCore.Mapping.Articles;
using Camino.Infrastructure.EntityFrameworkCore.Mapping.Farms;
using Camino.Infrastructure.EntityFrameworkCore.Mapping.Identities;
using Camino.Infrastructure.EntityFrameworkCore.Mapping.Media;
using Camino.Infrastructure.EntityFrameworkCore.Mapping.Products;
using Camino.Infrastructure.EntityFrameworkCore.Mapping.Navigations;
using Microsoft.EntityFrameworkCore;
using Camino.Core.Domains.Identifiers;
using Camino.Core.Domains.Navigations;
using Camino.Core.Domains.Products;
using Camino.Core.Domains.Orders;
using Camino.Core.Domains.Farms;
using Camino.Core.Domains.Media;
using Camino.Core.Domains.Articles;
using Camino.Core.Domains.Authorization;
using Camino.Core.Domains.Users;
using Camino.Core.Domains.Authentication;
using Camino.Core.Domains;

namespace Camino.Infrastructure.EntityFrameworkCore
{
    public class CaminoDbContext : EfDbContext, IAppDbContext, IDbContext
    {
        #region Dbset
        public DbSet<UserPhoto> UserPhotos { get; set; }
        public DbSet<UserPhotoType> UserPhotoTypes { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleCategory> ArticleCategories { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<ArticlePicture> ArticlePictures { get; set; }
        public DbSet<Farm> Farms { get; set; }
        public DbSet<FarmType> FarmTypes { get; set; }
        public DbSet<FarmPicture> FarmPictures { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductPicture> ProductPictures { get; set; }
        public DbSet<ProductCategoryRelation> ProductCategoryRelations { get; set; }
        public DbSet<ProductPrice> ProductPrices { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<FarmProduct> FarmProducts { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<ProductAttributeRelation> ProductAttributeRelations { get; set; }
        public DbSet<ProductAttributeRelationValue> ProductAttributeRelationValues { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<AuthorizationPolicy> AuthorizationPolicies { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<RoleAuthorizationPolicy> RoleAuthorizationPolicies { get; set; }
        public DbSet<RoleClaim> RoleClaims { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<UserAttribute> UserAttributes { get; set; }
        public DbSet<UserAuthorizationPolicy> UserAuthorizationPolicies { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
        public DbSet<Shortcut> Shortcuts { get; set; }
        #endregion

        #region Ctor

        public CaminoDbContext(DbContextOptions<CaminoDbContext> options) : base(options) { }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserPhotoMap())
                .ApplyConfiguration(new UserPhotoTypeMap())
                .ApplyConfiguration(new ArticleMap())
                .ApplyConfiguration(new ArticleCategoryMap())
                .ApplyConfiguration(new PictureMap())
                .ApplyConfiguration(new ArticlePictureMap())
                .ApplyConfiguration(new FarmMap())
                .ApplyConfiguration(new FarmTypeMap())
                .ApplyConfiguration(new FarmPictureMap())
                .ApplyConfiguration(new ProductMap())
                .ApplyConfiguration(new ProductCategoryMap())
                .ApplyConfiguration(new ProductPictureMap())
                .ApplyConfiguration(new ProductCategoryRelationMap())
                .ApplyConfiguration(new ProductPriceMap())
                .ApplyConfiguration(new OrderMap())
                .ApplyConfiguration(new OrderItemMap())
                .ApplyConfiguration(new FarmProductMap())
                .ApplyConfiguration(new ProductAttributeMap())
                .ApplyConfiguration(new ProductAttributeRelationMap())
                .ApplyConfiguration(new ProductAttributeRelationValueMap())
                .ApplyConfiguration(new UserMap())
                .ApplyConfiguration(new UserInfoMap())
                .ApplyConfiguration(new AuthorizationPolicyMap())
                .ApplyConfiguration(new CountryMap())
                .ApplyConfiguration(new GenderMap())
                .ApplyConfiguration(new RoleAuthorizationPolicyMap())
                .ApplyConfiguration(new RoleClaimMap())
                .ApplyConfiguration(new RoleMap())
                .ApplyConfiguration(new StatusMap())
                .ApplyConfiguration(new UserAttributeMap())
                .ApplyConfiguration(new UserAuthorizationPolicyMap())
                .ApplyConfiguration(new UserClaimMap())
                .ApplyConfiguration(new UserLoginMap())
                .ApplyConfiguration(new UserRoleMap())
                .ApplyConfiguration(new UserTokenMap())
                .ApplyConfiguration(new ShortcutMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}
