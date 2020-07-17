using Camino.Business.Contracts;
using Camino.Business.Implementation;
using Camino.Data.Contracts;
using Camino.Data.Entities.Content;
using Microsoft.Extensions.DependencyInjection;
using Camino.Business.ValidationStrategies;
using Camino.Business.Implementation.UserBusiness;
using Camino.Data.Entities.Identity;
using Camino.DAL;
using Camino.IdentityDAL;
using Camino.IdentityDAL.Implementations;
using Camino.DAL.Implementations;

namespace Camino.Business
{
    public static class BusinessStartup
    {
        public static void AddBusinessServices(this IServiceCollection services)
        {
            services.AddContentDataAccessServices("CaminoEntities");
            services.AddIdentityDataAccessServices("IdentityEntities");

            services.AddTransient<IRepository<User>, IdentityRepository<User>>()
                .AddTransient<IRepository<UserInfo>, IdentityRepository<UserInfo>>()
                .AddTransient<IRepository<Gender>, IdentityRepository<Gender>>()
                .AddTransient<IRepository<Country>, IdentityRepository<Country>>()
                .AddTransient<IRepository<Role>, IdentityRepository<Role>>()
                .AddTransient<IRepository<UserRole>, IdentityRepository<UserRole>>()
                .AddTransient<IRepository<UserAttribute>, IdentityRepository<UserAttribute>>()
                .AddTransient<IRepository<UserRole>, IdentityRepository<UserRole>>()
                .AddTransient<IRepository<AuthorizationPolicy>, IdentityRepository<AuthorizationPolicy>>()
                .AddTransient<IRepository<UserAuthorizationPolicy>, IdentityRepository<UserAuthorizationPolicy>>()
                .AddTransient<IRepository<RoleAuthorizationPolicy>, IdentityRepository<RoleAuthorizationPolicy>>()
                .AddTransient<IRepository<UserClaim>, IdentityRepository<UserClaim>>()
                .AddTransient<IRepository<UserToken>, IdentityRepository<UserToken>>()
                .AddTransient<IRepository<UserLogin>, IdentityRepository<UserLogin>>()
                .AddTransient<IRepository<RoleClaim>, IdentityRepository<RoleClaim>>();

            services.AddTransient<IUserBusiness, UserBusiness>()
                .AddTransient<ICountryBusiness, CountryBusiness>()
                .AddTransient<IUserPhotoBusiness, UserPhotoBusiness>()
                .AddTransient<IRoleBusiness, RoleBusiness>()
                .AddTransient<IUserAttributeBusiness, UserAttributeBusiness>()
                .AddTransient<IArticleCategoryBusiness, ArticleCategoryBusiness>()
                .AddTransient<IUserRoleBusiness, UserRoleBusiness>()
                .AddTransient<IAuthorizationPolicyBusiness, AuthorizationPolicyBusiness>()
                .AddTransient<IRoleAuthorizationPolicyBusiness, RoleAuthorizationPolicyBusiness>()
                .AddTransient<IUserAuthorizationPolicyBusiness, UserAuthorizationPolicyBusiness>()
                .AddTransient<IUserClaimBusiness, UserClaimBusiness>()
                .AddTransient<IUserTokenBusiness, UserTokenBusiness>()
                .AddTransient<IUserLoginBusiness, UserLoginBusiness>()
                .AddTransient<IRoleClaimBusiness, RoleClaimBusiness>()
                .AddTransient<ISeedDataBusiness, SeedDataBusiness>();

            services.AddTransient<IRepository<Product>, ContentRepository<Product>>()
                .AddTransient<IRepository<ArticleCategory>, ContentRepository<ArticleCategory>>()
                .AddTransient<IRepository<UserPhoto>, ContentRepository<UserPhoto>>()
                .AddTransient<ValidationStrategyContext>();
        }
    }
}
