using Coco.Api.Framework.Security;
using Coco.Api.Framework.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Coco.Api.Framework
{
    public static class FrameworkStartup
    {
        public static IdentityBuilder AddCustomStores(this IdentityBuilder builder)
        {
            builder.Services.AddTransient<IPasswordValidator<ApplicationUser>, CustomPasswordValidator>();
            builder.Services.AddTransient<IUserValidator<ApplicationUser>, CustomUserValidator>();

            builder.Services.AddTransient<UserManager<ApplicationUser>, CustomUserManager>();
            builder.Services.AddTransient<SignInManager<ApplicationUser>, CustomSignInManager>();

            builder.Services.AddTransient<IPasswordHasher<ApplicationUser>, TextHasher>();

            builder.Services.AddTransient<IUserStore<ApplicationUser>, CustomUserStore>();
            builder.Services.AddTransient<IUserPasswordStore<ApplicationUser>, CustomUserStore>();
            builder.Services.AddTransient<IUserEmailStore<ApplicationUser>, CustomUserStore>();

            builder.Services.AddTransient<IRoleStore<ApplicationRole>, CustomRoleStore>();

            return builder;
        }
    }
}
