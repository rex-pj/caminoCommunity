using Coco.Common.Const;
using Coco.Framework.Models;
using Coco.Framework.Services.Contracts;
using Coco.Framework.Services.Implementation;
using Coco.Framework.SessionManager;
using Coco.Framework.SessionManager.Contracts;
using Coco.Framework.SessionManager.Core;
using Coco.Framework.SessionManager.Stores;
using Coco.Framework.SessionManager.Stores.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Coco.Framework.Infrastructure.Extensions
{
    public static class IdentityServiceCollectionExtensions
    {
        public static void AddUserIdentity(this IServiceCollection services)
        {
            services.AddUserIdentity(setupAction: null);
        }

        public static void AddUserIdentity(this IServiceCollection services, Action<IdentityOptions> setupAction)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>(x =>
                {
                    x.Password.RequireDigit = true;
                    x.Password.RequireLowercase = true;
                    x.Password.RequireUppercase = true;
                    x.Password.RequireNonAlphanumeric = true;
                    x.Password.RequiredLength = 6;
                })
                .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>(ServiceProvidersNameConst.COCO_API_AUTH)
                .AddDefaultTokenProviders();

            services
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddTransient<IUserManager<ApplicationUser>, ApplicationUserManager<ApplicationUser>>()
                .AddTransient<ILoginManager<ApplicationUser>, ApplicationLoginManager<ApplicationUser>>()
                .AddTransient<ISessionRoleManager<ApplicationRole>, ApplicationRoleManager<ApplicationRole>>()
                .AddTransient<IUserEncryptionStore<ApplicationUser>, ApplicationUserStore>()
                //.AddTransient<ISessionRoleManager<ApplicationRole>, SessionRoleManager>()
                //.AddTransient<ISessionRoleStore<ApplicationRole>, SessionRoleStore>()
                .AddTransient<IUserStore<ApplicationUser>, ApplicationUserStore>()
                .AddTransient<IUserPasswordStore<ApplicationUser>, ApplicationUserStore>()
                .AddTransient<IRoleStore<ApplicationRole>, SessionRoleStore>()
                //.AddScoped<ISessionClaimsPrincipalFactory<ApplicationUser>, SessionClaimsPrincipalFactory<ApplicationUser, ApplicationRole>>()
                .AddTransient<ITextEncryption, TextEncryption>()
                .AddTransient(typeof(IUserAttributeStore<>), typeof(UserAttributeStore<>))
                .AddScoped<ISessionContext, SessionContext>()
                .AddScoped<IEmailSender, EmailSender>()
                .AddScoped<SessionState>();

            if (setupAction != null)
            {
                services.Configure(setupAction);
            }
        }
    }
}
