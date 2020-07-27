using Camino.Core.Constants;
using Camino.Framework.Models;
using Camino.Framework.Providers.Contracts;
using Camino.Framework.Providers.Implementation;
using Camino.Framework.SessionManager;
using Camino.Framework.SessionManager.Contracts;
using Camino.Framework.SessionManager.Core;
using Camino.Framework.SessionManager.Stores;
using Camino.Framework.SessionManager.Stores.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Camino.Framework.Infrastructure.Extensions
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration.GetSection(AppSettings.Name));
            services.Configure<CrypterSettings>(configuration.GetSection(CrypterSettings.Name));
            services.Configure<EmailSenderSettings>(configuration.GetSection(EmailSenderSettings.Name));
            services.AddIdentity<ApplicationUser, ApplicationRole>(x =>
                {
                    x.Password.RequireDigit = true;
                    x.Password.RequireLowercase = true;
                    x.Password.RequireUppercase = true;
                    x.Password.RequireNonAlphanumeric = true;
                    x.Password.RequiredLength = 6;
                })
                .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>(ServiceProvidersNameConst.CAMINO_API_AUTH)
                .AddDefaultTokenProviders();

            services
                .AddSingleton<IFileProvider, FileProvider>()
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddTransient<IUserManager<ApplicationUser>, ApplicationUserManager<ApplicationUser>>()
                .AddTransient<ILoginManager<ApplicationUser>, ApplicationLoginManager<ApplicationUser>>()
                .AddTransient<IApplicationRoleManager<ApplicationRole>, ApplicationRoleManager<ApplicationRole>>()
                .AddTransient<IUserEncryptionStore<ApplicationUser>, ApplicationUserStore>()
                .AddTransient<IUserStore<ApplicationUser>, ApplicationUserStore>()
                .AddTransient<IUserPasswordStore<ApplicationUser>, ApplicationUserStore>()
                .AddTransient<IUserSecurityStampStore<ApplicationUser>, ApplicationUserStore>()
                .AddTransient<IRoleStore<ApplicationRole>, ApplicationRoleStore>()
                .AddTransient<ITextEncryption, TextEncryption>()
                .AddScoped<ISessionContext, SessionContext>()
                .AddScoped<IEmailProvider, EmailProvider>()
                .AddScoped<SessionState>();
        }
    }
}
