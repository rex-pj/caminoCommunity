using Coco.Api.Framework.Models;
using Coco.Api.Framework.Services.Contracts;
using Coco.Api.Framework.Services.Implementation;
using Coco.Api.Framework.SessionManager;
using Coco.Api.Framework.SessionManager.Contracts;
using Coco.Api.Framework.SessionManager.Core;
using Coco.Api.Framework.SessionManager.Stores;
using Coco.Api.Framework.SessionManager.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Coco.Api.Framework.Infrastructure.Extensions
{
    public static class IdentityServiceCollectionExtensions
    {
        public static void AddUserIdentity(this IServiceCollection services)
        {
            services.AddUserIdentity(setupAction: null);
        }

        public static void AddUserIdentity(
            this IServiceCollection services, Action<IdentityOptions> setupAction)
        {
            services
                .AddTransient<ILookupNormalizer, LookupNormalizer>()
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddTransient<IPasswordValidator<ApplicationUser>, PasswordValidator>()
                .AddTransient<IUserValidator<ApplicationUser>, UserValidator>()
                .AddTransient<IUserManager<ApplicationUser>, UserManager>()
                .AddTransient<ILoginManager<ApplicationUser>, LoginManager>()
                .AddTransient<IPasswordHasher<ApplicationUser>, PasswordHasher>()
                .AddTransient<IUserPasswordStore<ApplicationUser>, UserPasswordStore>()
                .AddTransient<IUserStore<ApplicationUser>, UserStore>()
                .AddTransient<IUserPhotoStore<ApplicationUser>, UserPhotoStore>()
                .AddTransient<IUserEmailStore<ApplicationUser>, UserEmailStore>()
                .AddTransient<ITextCrypter, TextCrypter>()
                .AddTransient(typeof(IUserStampStore<>), typeof(UserStampStore<>))
                .AddScoped<ISessionContext, SessionContext>()
                .AddScoped<IEmailSender, EmailSender>();

            if (setupAction != null)
            {
                services.Configure(setupAction);
            }
        }
    }
}
