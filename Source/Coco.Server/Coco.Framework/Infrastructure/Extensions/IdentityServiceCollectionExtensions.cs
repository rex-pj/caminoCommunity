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
                .AddTransient<IApplicationRoleManager<ApplicationRole>, ApplicationRoleManager<ApplicationRole>>()
                .AddTransient<IUserEncryptionStore<ApplicationUser>, ApplicationUserStore>()
                .AddTransient<IUserStore<ApplicationUser>, ApplicationUserStore>()
                .AddTransient<IUserPasswordStore<ApplicationUser>, ApplicationUserStore>()
                .AddTransient<IRoleStore<ApplicationRole>, ApplicationRoleStore>()
                .AddTransient<ITextEncryption, TextEncryption>()
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
