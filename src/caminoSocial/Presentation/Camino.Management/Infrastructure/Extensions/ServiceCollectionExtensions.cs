using AutoMapper;
using Camino.Business;
using Camino.Business.AutoMap;
using Camino.Framework.Infrastructure.AutoMap;
using Camino.Framework.Infrastructure.Extensions;
using Camino.Framework.Models;
using Camino.Framework.Providers.Contracts;
using Camino.Framework.Providers.Implementation;
using Camino.Framework.SessionManager.Core;
using Camino.Management.Infrastructure.AutoMap;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Camino.Management.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureManagementServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(FrameworkMappingProfile), typeof(ContentMappingProfile), typeof(IdentityMappingProfile), typeof(AuthMappingProfile));
            services.AddApplicationServices(configuration);
            services.AddAuthentication(IdentitySettings.APP_SESSION_SCHEMA).AddCookie(IdentitySettings.APP_SESSION_SCHEMA);
            services.AddBusinessServices();

            services.AddScoped<ISetupProvider, SetupProvider>();
            services.AddSingleton<SetupSettings>();

            return services;
        }
    }
}
