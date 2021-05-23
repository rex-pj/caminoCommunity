using Camino.Core.Contracts.Data;
using Camino.Infrastructure.Data;
using Camino.Infrastructure.Strategies.Validations;
using LinqToDB.AspNet;
using LinqToDB.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Camino.Services.Articles;
using Camino.Services.Authentication;
using Camino.Services.Authorization;
using Camino.Services.Farms;
using Camino.Services.Feeds;
using Camino.Services.Identifiers;
using Camino.Services.Media;
using Camino.Services.Products;
using Camino.Services.Setup;
using Camino.Services.Users;
using Camino.Core.Contracts.Services.Users;
using Camino.Core.Contracts.Services.Authentication;
using Camino.Core.Contracts.Services.Identities;
using Camino.Core.Contracts.Services.Articles;
using Camino.Core.Contracts.Services.Authorization;
using Camino.Core.Contracts.Services.Setup;
using Camino.Core.Contracts.Services.Farms;
using Camino.Core.Contracts.Services.Products;
using Camino.Core.Contracts.Services.Media;
using Camino.Core.Contracts.Services.Feeds;
using Camino.Core.Contracts.Repositories.Users;
using Camino.Service.Repository.Identifiers;
using Camino.Core.Contracts.Repositories.Authentication;
using Camino.Service.Repository.Users;
using Camino.Core.Contracts.Repositories.Authorization;
using Camino.Service.Repository.Authorization;
using Camino.Service.Repository.Articles;
using Camino.Core.Contracts.Repositories.Articles;
using Camino.Core.Contracts.Repositories.Identities;
using Camino.Service.Repository.Authentication;
using Camino.Core.Contracts.Repositories.Setup;
using Camino.Service.Repository.Setup;
using Camino.Core.Contracts.Repositories.Products;
using Camino.Core.Contracts.Repositories.Media;
using Camino.Service.Repository.Farms;
using Camino.Core.Contracts.Repositories.Farms;
using Camino.Service.Repository.Media;
using Camino.Service.Repository.Products;
using Camino.Service.Repository.Feeds;
using Camino.Core.Contracts.Repositories.Feeds;
using LinqToDB;
using Camino.Infrastructure.Repositories.Navigations;
using Camino.Core.Contracts.Repositories.Navigations;
using Camino.Core.Services.Navigations;
using Camino.Core.Contracts.Services.Navigations;

namespace Camino.Infrastructure.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddTransient<ValidationStrategyContext>();
            services.AddRepositoryServices();
        }

        public static void AddRepositoryServices(this IServiceCollection services)
        {
            services.AddDataAccessServices();
            services.AddTransient(typeof(IRepository<>), typeof(CaminoRepository<>));
            services.AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IAuthenticationRepository, AuthenticationRepository>()
                .AddScoped<IUserStatusRepository, UserStatusRepository>()
                .AddScoped<ICountryRepository, CountryRepository>()
                .AddScoped<IUserPhotoRepository, UserPhotoRepository>()
                .AddScoped<IRoleRepository, RoleRepository>()
                .AddScoped<IUserAttributeRepository, UserAttributeRepository>()
                .AddScoped<IArticleCategoryRepository, ArticleCategoryRepository>()
                .AddScoped<IArticlePictureRepository, ArticlePictureRepository>()
                .AddScoped<IUserRoleRepository, UserRoleRepository>()
                .AddScoped<IAuthorizationPolicyRepository, AuthorizationPolicyRepository>()
                .AddScoped<IRoleAuthorizationPolicyRepository, RoleAuthorizationPolicyRepository>()
                .AddScoped<IUserAuthorizationPolicyRepository, UserAuthorizationPolicyRepository>()
                .AddScoped<IUserClaimRepository, UserClaimRepository>()
                .AddScoped<IUserTokenRepository, UserTokenRepository>()
                .AddScoped<IUserLoginRepository, UserLoginRepository>()
                .AddScoped<IRoleClaimRepository, RoleClaimRepository>()
                .AddScoped<IDbCreationRepository, DbCreationRepository>()
                .AddScoped<IDataSeedRepository, DataSeedRepository>()
                .AddScoped<IArticleRepository, ArticleRepository>()
                .AddScoped<IPictureRepository, PictureRepository>()
                .AddScoped<IFarmRepository, FarmRepository>()
                .AddScoped<IFarmTypeRepository, FarmTypeRepository>()
                .AddScoped<IFarmPictureRepository, FarmPictureRepository>()
                .AddScoped<IProductRepository, ProductRepository>()
                .AddScoped<IProductCategoryRepository, ProductCategoryRepository>()
                .AddScoped<IProductPictureRepository, ProductPictureRepository>()
                .AddScoped<IFeedRepository, FeedRepository>()
                .AddScoped<IProductAttributeRepository, ProductAttributeRepository>()
                .AddScoped<IShortcutRepository, ShortcutRepository>();
        }

        public static void AddDataAccessServices(this IServiceCollection services)
        {
            services.AddDbConnectionServices<CaminoDataConnection>("CaminoEntities");
        }

        public static void AddDbConnectionServices<TContext>(this IServiceCollection services, string connectionKey) where TContext : IDataContext
        {
            var configuration = services.BuildServiceProvider()
                .GetRequiredService<IConfiguration>();

            services.AddLinqToDbContext<TContext>((provider, options) =>
            {
                options.UseSqlServer(configuration.GetConnectionString(connectionKey));
            });
        }

        public static void AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>()
                .AddScoped<IAuthenticationService, AuthenticationService>()
                .AddScoped<IUserStatusService, UserStatusService>()
                .AddScoped<ICountryService, CountryService>()
                .AddScoped<IUserPhotoService, UserPhotoService>()
                .AddScoped<IRoleService, RoleService>()
                .AddScoped<IUserAttributeService, UserAttributeService>()
                .AddScoped<IArticleCategoryService, ArticleCategoryService>()
                .AddScoped<IUserRoleService, UserRoleService>()
                .AddScoped<IAuthorizationPolicyService, AuthorizationPolicyService>()
                .AddScoped<IRoleAuthorizationPolicyService, RoleAuthorizationPolicyService>()
                .AddScoped<IUserAuthorizationPolicyService, UserAuthorizationPolicyService>()
                .AddScoped<IRoleClaimService, RoleClaimService>()
                .AddScoped<IDataSeedService, DataSeedService>()
                .AddScoped<IArticleService, ArticleService>()
                .AddScoped<IPictureService, PictureService>()
                .AddScoped<IFarmService, FarmService>()
                .AddScoped<IFarmTypeService, FarmTypeService>()
                .AddScoped<IProductService, ProductService>()
                .AddScoped<IProductCategoryService, ProductCategoryService>()
                .AddScoped<IFeedService, FeedService>()
                .AddScoped<IProductAttributeService, ProductAttributeService>()
                .AddScoped<IShortcutService, ShortcutService>();
        }
    }
}
