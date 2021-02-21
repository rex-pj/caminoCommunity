using Camino.Framework.Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Camino.IdentityManager.Contracts.Core;
using Camino.Shared.Configurations;
using Camino.Infrastructure.Infrastructure.Extensions;

namespace Camino.Management.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureManagementServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationServices(configuration);
            services.AddAuthentication(IdentitySettings.APP_SESSION_SCHEMA).AddCookie(IdentitySettings.APP_SESSION_SCHEMA);
            services.AddInfrastructureServices();
            services.AddDomainServices();
            services.AddDataProtection();

            services.AddSingleton<SetupSettings>();

            return services;
        }
    }
}
