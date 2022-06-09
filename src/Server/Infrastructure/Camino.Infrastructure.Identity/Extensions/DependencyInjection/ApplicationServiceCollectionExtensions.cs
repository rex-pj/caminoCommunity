using Camino.IdentityManager.Contracts.Stores;
using Camino.Infrastructure.Identity;
using Camino.Infrastructure.Identity.Constants;
using Camino.Infrastructure.Identity.Core;
using Camino.Infrastructure.Identity.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Camino.IdentityManager.Extensions.DependencyInjection
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationIdentity<TUser, TRole>(this IServiceCollection services)
            where TUser : ApplicationUser
            where TRole : IdentityRole<long>
        {
            services.AddIdentity<TUser, TRole>(x =>
                {
                    x.Password.RequireDigit = true;
                    x.Password.RequireLowercase = true;
                    x.Password.RequireUppercase = true;
                    x.Password.RequireNonAlphanumeric = true;
                    x.Password.RequiredLength = 6;
                })
                .AddTokenProvider<DataProtectorTokenProvider<TUser>>(ServiceProvidersNames.CAMINO_API_AUTH)
                .AddDefaultTokenProviders();

            services.AddScoped<IUserManager<TUser>, ApplicationUserManager<TUser>>()
                .AddScoped<ILoginManager<TUser>, ApplicationLoginManager<TUser>>()
                .AddScoped<IApplicationRoleManager<TRole>, ApplicationRoleManager<TRole>>()
                .AddScoped<IUserEncryptionStore<TUser>, ApplicationUserStore<TUser>>()
                .AddScoped<IUserTokenStore<TUser>, ApplicationUserStore<TUser>>()
                .AddScoped<IUserPolicyStore<TUser>, ApplicationUserStore<TUser>>()
                .AddScoped<IUserStore<TUser>, ApplicationUserStore<TUser>>()
                .AddScoped<IUserPasswordStore<TUser>, ApplicationUserStore<TUser>>()
                .AddScoped<IUserSecurityStampStore<TUser>, ApplicationUserStore<TUser>>()
                .AddScoped<IRoleStore<TRole>, ApplicationRoleStore<TRole>>()
                .AddScoped<ITextEncryption, TextEncryption>();

            return services;
        }
    }
}
