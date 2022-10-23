using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Camino.Shared.Configuration.Options;
using Camino.Infrastructure.Identity.Core;
using Camino.Infrastructure.Http.Interfaces;
using Camino.Infrastructure.Identity;
using Camino.Infrastructure.Http;
using Camino.Infrastructure.Identity.Interfaces;
using Camino.Infrastructure.Providers;
using Camino.Infrastructure.Files.Contracts;
using Camino.Core.Domains;
using Camino.Infrastructure.Identity.Options;
using Camino.Infrastructure.Identity.Extensions.DependencyInjection;
using Camino.Infrastructure.EntityFrameworkCore;

namespace Camino.Infrastructure.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped(typeof(IEntityRepository<>), typeof(EfRepository<>));
            services.AddProjectDependencies();
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
