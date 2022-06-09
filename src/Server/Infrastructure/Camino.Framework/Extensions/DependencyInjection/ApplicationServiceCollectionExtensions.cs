using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Camino.IdentityManager.Extensions.DependencyInjection;
using Camino.IdentityManager.Contracts.Options;
using Camino.Shared.Configuration.Options;
using Camino.Infrastructure.Identity.Core;
using Camino.Infrastructure.Http.Interfaces;
using Camino.Infrastructure.Identity;
using Camino.Infrastructure.Http;
using Camino.Infrastructure.Identity.Interfaces;

namespace Camino.Framework.Extensions.DependencyInjection
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration.GetSection(AppSettings.Name));
            services.Configure<CrypterSettings>(configuration.GetSection(CrypterSettings.Name));
            services.Configure<EmailSenderSettings>(configuration.GetSection(EmailSenderSettings.Name));
            services.Configure<PagerOptions>(configuration.GetSection(PagerOptions.Name));
            services.Configure<JwtConfigOptions>(configuration.GetSection(JwtConfigOptions.Name));

            services.AddApplicationIdentity<ApplicationUser, ApplicationRole>()
                .AddScoped<IHttpHelper, HttpHelper>()
                .AddScoped<IJwtHelper, JwtHelper>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            return services;
        }
    }
}
