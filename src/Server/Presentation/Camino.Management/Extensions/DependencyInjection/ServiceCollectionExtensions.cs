using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Camino.Framework.Extensions.DependencyInjection;
using Camino.Infrastructure.Extensions.DependencyInjection;
using Camino.Framework.Infrastructure.ModelBinders;
using Camino.Shared.Configuration.Options;
using Camino.IdentityManager.Contracts.Options;
using Camino.Infrastructure.EntityFrameworkCore.Extensions.DependencyInjection;
using Camino.Infrastructure.EntityFrameworkCore;
using Camino.Core.Domains;

namespace Camino.Management.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureManagementServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationServices(configuration);
            services.AddAuthentication(IdentitySettings.APP_SESSION_SCHEMA).AddCookie(IdentitySettings.APP_SESSION_SCHEMA);
            services.AddInfrastructureServices();
            services.AddDataAccessServices<CaminoDbContext>();
            services.AddDataProtection();

            services.AddSingleton<SetupSettings>();

            services.AddControllersWithViews(options =>
            {
                options.ModelBinderProviders.Insert(0, new ApplicationModelBinderProvider());
            })
            .AddNewtonsoftJson()
            .AddModularManager()
            .AddModules(configuration);

            services.AddAutoMappingModular();

            return services;
        }
    }
}
