using Camino.Core.Constants;
using Camino.IdentityManager.Contracts;
using Camino.IdentityManager.Contracts.Stores;
using Camino.IdentityManager.Contracts.Stores.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Camino.IdentityManager.Infrastructure.Extensions
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationIdentity<TUser, TRole>(this IServiceCollection services)
            where TUser : IdentityUser<long>
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
                .AddTokenProvider<DataProtectorTokenProvider<TUser>>(ServiceProvidersNameConst.CAMINO_API_AUTH)
                .AddDefaultTokenProviders();

            services.AddTransient<IUserManager<TUser>, ApplicationUserManager<TUser>>()
                .AddTransient<ILoginManager<TUser>, ApplicationLoginManager<TUser>>()
                .AddTransient<IApplicationRoleManager<TRole>, ApplicationRoleManager<TRole>>()
                .AddTransient<IUserEncryptionStore<TUser>, ApplicationUserStore<TUser>>()
                .AddTransient<IUserPolicyStore<TUser>, ApplicationUserStore<TUser>>()
                .AddTransient<IUserStore<TUser>, ApplicationUserStore<TUser>>()
                .AddTransient<IUserPasswordStore<TUser>, ApplicationUserStore<TUser>>()
                .AddTransient<IUserSecurityStampStore<TUser>, ApplicationUserStore<TUser>>()
                .AddTransient<IRoleStore<TRole>, ApplicationRoleStore<TRole>>()
                .AddTransient<ITextEncryption, TextEncryption>()
                .AddScoped<ISessionContext, SessionContext>();

            return services;
        }
    }
}
