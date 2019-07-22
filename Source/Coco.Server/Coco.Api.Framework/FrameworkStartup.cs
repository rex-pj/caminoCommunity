using Coco.Api.Framework.AccountIdentity;
using Coco.Api.Framework.Models;
using Microsoft.Extensions.DependencyInjection;
using Coco.Api.Framework.AccountIdentity.Contracts;
using Microsoft.AspNetCore.Http;

namespace Coco.Api.Framework
{
    public static class FrameworkStartup
    {
        public static void AddCustomStores(IServiceCollection services)
        {
            services.AddTransient<ILookupNormalizer, LookupNormalizer>()
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddTransient<IPasswordValidator<ApplicationUser>, PasswordValidator>()
                .AddTransient<IUserValidator<ApplicationUser>, UserValidator>()
                .AddTransient<IAccountManager<ApplicationUser>, AccountManager>()
                .AddTransient<ILoginManager<ApplicationUser>, LoginManager>()
                .AddTransient<IPasswordHasher<ApplicationUser>, PasswordHasher>()
                .AddTransient<IUserPasswordStore<ApplicationUser>, UserPasswordStore>()
                .AddTransient<IUserStore<ApplicationUser>, UserStore>()
                .AddTransient<IUserEmailStore<ApplicationUser>, UserEmailStore>()
                .AddTransient<ILookupProtectorKeyRing, DefaultKeyRing>()
                .AddTransient<ILookupProtector, SillyEncryptor>()
                .AddTransient<ITextCrypter, TextCrypter>()
                .AddScoped<IWorkContext, WorkContext>();
        }
    }
}
