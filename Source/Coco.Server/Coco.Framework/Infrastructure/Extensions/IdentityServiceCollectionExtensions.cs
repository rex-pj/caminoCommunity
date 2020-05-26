using Coco.Framework.Models;
using Coco.Framework.Services.Contracts;
using Coco.Framework.Services.Implementation;
using Coco.Framework.SessionManager;
using Coco.Framework.SessionManager.Contracts;
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
            services.AddIdentity<ApplicationUser, ApplicationRole>(o =>
                {
                    o.Password.RequireDigit = true;
                    o.Password.RequireLowercase = true;
                    o.Password.RequireUppercase = true;
                    o.Password.RequireNonAlphanumeric = true;
                    o.Password.RequiredLength = 6;
                })
                .AddTokenProvider("Coco.Api.Auth", typeof(DataProtectorTokenProvider<ApplicationUser>))
                .AddDefaultTokenProviders();

            services
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddTransient<IUserManager<ApplicationUser>, ApplicationUserManager<ApplicationUser>>()
                .AddTransient<ILoginManager<ApplicationUser>, ApplicationLoginManager<ApplicationUser>>()
                .AddTransient<ISessionRoleManager<ApplicationRole>, ApplicationRoleManager<ApplicationRole>>()
                //.AddTransient<ISessionRoleManager<ApplicationRole>, SessionRoleManager>()
                //.AddTransient<ISessionRoleStore<ApplicationRole>, SessionRoleStore>()
                //.AddTransient<IPasswordHasher<ApplicationUser>, PasswordHasher>()
                //.AddTransient<IUserPasswordStore<ApplicationUser>, UserPasswordStore>()
                .AddTransient<IUserStore<ApplicationUser>, ApplicationUserStore>()
                .AddTransient<IUserPasswordStore<ApplicationUser>, ApplicationUserStore>()
                .AddTransient<IRoleStore<ApplicationRole>, SessionRoleStore>()
                //.AddTransient<IUserEmailStore<ApplicationUser>, UserEmailStore>()
                //.AddScoped<ISessionClaimsPrincipalFactory<ApplicationUser>, SessionClaimsPrincipalFactory<ApplicationUser, ApplicationRole>>()
                //.AddTransient<ITextCrypter, TextCrypter>()
                .AddTransient(typeof(IUserAttributeStore<>), typeof(UserAttributeStore<>))
                .AddTransient<ISessionContext, SessionContext>()
                .AddScoped<ITextRandom, TextRandom>()
                .AddScoped<IEmailSender, EmailSender>();

            if (setupAction != null)
            {
                services.Configure(setupAction);
            }
        }
    }
}
