using Coco.Framework.Models;
using Coco.Framework.Services.Contracts;
using Coco.Framework.Services.Implementation;
using Coco.Framework.SessionManager;
using Coco.Framework.SessionManager.Contracts;
using Coco.Framework.SessionManager.Core;
using Coco.Framework.SessionManager.Stores;
using Coco.Framework.SessionManager.Validators;
using Microsoft.AspNetCore.Http;
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

        public static void AddUserIdentity(
            this IServiceCollection services, Action<IdentityOptions> setupAction)
        {
            services
                .AddTransient<ILookupNormalizer, LookupNormalizer>()
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddTransient<IPasswordValidator<ApplicationUser>, PasswordValidator>()
                .AddTransient<IUserValidator<ApplicationUser>, UserValidator>()
                .AddTransient<IUserManager<ApplicationUser>, UserManager>()
                .AddTransient<IPasswordHasher<ApplicationUser>, PasswordHasher>()
                .AddTransient<IUserPasswordStore<ApplicationUser>, UserPasswordStore>()
                .AddTransient<IUserStore<ApplicationUser>, UserStore>()
                .AddTransient<IUserPhotoStore<ApplicationUser>, UserPhotoStore>()
                .AddTransient<IUserEmailStore<ApplicationUser>, UserEmailStore>()
                .AddTransient<ITextCrypter, TextCrypter>()
                .AddTransient(typeof(IUserStampStore<>), typeof(UserStampStore<>))
                .AddTransient<ISessionContext, SessionContext>()
                .AddScoped<IEmailSender, EmailSender>();

            if (setupAction != null)
            {
                services.Configure(setupAction);
            }
        }
    }
}
