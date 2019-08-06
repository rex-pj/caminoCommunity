using Coco.Api.Framework.UserIdentity;
using Coco.Api.Framework.Models;
using Microsoft.Extensions.DependencyInjection;
using Coco.Api.Framework.UserIdentity.Contracts;
using Microsoft.AspNetCore.Http;
using Coco.Api.Framework.UserIdentity.Validators;
using Coco.Api.Framework.UserIdentity.Stores;
using AutoMapper;
using Coco.Api.Framework.MappingProfiles;

namespace Coco.Api.Framework
{
    public static class FrameworkStartup
    {
        public static void AddCustomStores(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(UserMappingProfile));

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
                .AddScoped<ISessionContext, SessionContext>();
        }
    }
}
