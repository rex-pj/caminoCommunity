using Camino.Framework.Infrastructure.Contracts;
using Camino.Framework.Models.Settings;
using Camino.Framework.Providers;
using Camino.Framework.Providers.Contracts;
using Camino.IdentityManager.Contracts.Core;
using Camino.IdentityManager.Infrastructure.Extensions;
using Camino.IdentityManager.Models;
using Microsoft.AspNetCore.Http;
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

            services.AddApplicationIdentity<ApplicationUser, ApplicationRole>();
            services.AddTransient<IPageAuthorizeManager, PageAuthorizeManager>();

            services
                .AddSingleton<IFileProvider, FileProvider>()
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddScoped<IEmailProvider, EmailProvider>()
                .AddScoped<SessionState>();
        }
    }
}
