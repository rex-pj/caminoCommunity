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
using Camino.Infrastructure.Providers;
using Camino.Infrastructure.Files.Contracts;
using Camino.Core.Domains;
using Camino.Infrastructure.EntityFrameworkCore;
using Camino.Infrastructure.Extensions.DependencyInjection;

namespace Camino.Framework.Extensions.DependencyInjection
{
    public static class ApplicationServiceCollectionExtensions
    {
        internal static readonly string[] _dependencyProjectNames = new string[]
        {
            "Camino.Infrastructure",
            "Camino.Infrastructure.Emails",
            "Camino.Infrastructure.Files",
            "Camino.Infrastructure.Http",
            "Camino.Infrastructure.EntityFrameworkCore",
            "Camino.Core",
            "Camino.Application",
        };

        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped(typeof(IEntityRepository<>), typeof(EfRepository<>));
            services.AddDependencyServices(_dependencyProjectNames);
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration.GetSection(AppSettings.Name));
            services.Configure<CrypterSettings>(configuration.GetSection(CrypterSettings.Name));
            services.Configure<EmailSenderSettings>(configuration.GetSection(EmailSenderSettings.Name));
            services.Configure<PagerOptions>(configuration.GetSection(PagerOptions.Name));
            services.Configure<JwtConfigOptions>(configuration.GetSection(JwtConfigOptions.Name));

            services.AddApplicationIdentity<ApplicationUser, ApplicationRole>()
                .AddScoped<IHttpHelper, HttpHelper>()
                .AddScoped<IFileProvider, FileProvider>()
                .AddScoped<IJwtHelper, JwtHelper>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            return services;
        }
    }
}
