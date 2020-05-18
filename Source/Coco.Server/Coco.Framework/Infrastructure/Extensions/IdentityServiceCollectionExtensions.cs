using Coco.Framework.Services.Contracts;
using Coco.Framework.Services.Implementation;
using Coco.Framework.SessionManager;
using Coco.Framework.SessionManager.Contracts;
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
            services
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                //.AddTransient<IUserManager<ApplicationUser>, ApplicationUserManager>()
                //.AddTransient<ISessionRoleManager<ApplicationRole>, SessionRoleManager>()
                //.AddTransient<ISessionRoleStore<ApplicationRole>, SessionRoleStore>()
                //.AddTransient<IPasswordHasher<ApplicationUser>, PasswordHasher>()
                //.AddTransient<IUserPasswordStore<ApplicationUser>, UserPasswordStore>()
                //.AddTransient<IUserStore<ApplicationUser>, UserStore>()
                //.AddTransient<IUserEmailStore<ApplicationUser>, UserEmailStore>()
                //.AddScoped<ISessionClaimsPrincipalFactory<ApplicationUser>, SessionClaimsPrincipalFactory<ApplicationUser, ApplicationRole>>()
                //.AddTransient<ITextCrypter, TextCrypter>()
                //.AddTransient(typeof(IUserStampStore<>), typeof(UserStampStore<>))
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
