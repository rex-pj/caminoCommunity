using Camino.Framework.Helpers;
using Camino.Shared.Configurations;
using Camino.Infrastructure.Providers;
using Camino.Core.Contracts.Providers;
using Camino.IdentityManager.Contracts.Core;
using Camino.IdentityManager.Infrastructure.Extensions;
using Camino.Core.Domain.Identities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Camino.Core.Contracts.Helpers;

namespace Camino.Framework.Infrastructure.Extensions
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration.GetSection(AppSettings.Name));
            services.Configure<CrypterSettings>(configuration.GetSection(CrypterSettings.Name));
            services.Configure<EmailSenderSettings>(configuration.GetSection(EmailSenderSettings.Name));

            services.AddApplicationIdentity<ApplicationUser, ApplicationRole>()
                .AddTransient<IHttpHelper, HttpHelper>();

            services
                .AddSingleton<IFileProvider, FileProvider>()
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddScoped<IEmailProvider, EmailProvider>();
        }
    }
}
