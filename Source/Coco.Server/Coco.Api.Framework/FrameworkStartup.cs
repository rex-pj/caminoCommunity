using Coco.Api.Framework.AccountIdentity;
using Coco.Api.Framework.Models;
using Microsoft.Extensions.DependencyInjection;
using Coco.Api.Framework.AccountIdentity.Contracts;

namespace Coco.Api.Framework
{
    public static class FrameworkStartup
    {
        public static void AddCustomStores(IServiceCollection services)
        {
            services.AddTransient<ILookupNormalizer, LookupNormalizer>();
            services.AddTransient<IPasswordValidator<ApplicationUser>, PasswordValidator>();
            services.AddTransient<IUserValidator<ApplicationUser>, UserValidator>();

            services.AddTransient<IAccountManager<ApplicationUser>, AccountManager>();
            services.AddTransient<ILoginManager<ApplicationUser>, LoginManager>();

            services.AddTransient<IPasswordHasher<ApplicationUser>, PasswordHasher>();

            services.AddTransient<IUserPasswordStore<ApplicationUser>, UserPasswordStore>();
            services.AddTransient<IUserStore<ApplicationUser>, UserStore>();
            services.AddTransient<IUserEmailStore<ApplicationUser>, UserEmailStore>();
            services.AddTransient<ILookupProtectorKeyRing, DefaultKeyRing>();
            services.AddTransient<ILookupProtector, SillyEncryptor>();
            services.AddTransient<ITextCrypter, TextCrypter>();
        }
    }
}
