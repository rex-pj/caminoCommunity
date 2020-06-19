using AutoMapper;
using Coco.Business;
using Coco.Business.AutoMap;
using Coco.Framework.Infrastructure.AutoMap;
using Coco.Framework.Infrastructure.Extensions;
using Coco.Framework.Models;
using Coco.Framework.Providers.Contracts;
using Coco.Framework.Providers.Implementation;
using Coco.Framework.SessionManager.Core;
using Coco.Management.Infrastructure.AutoMap;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Coco.Management.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureManagementServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(FrameworkMappingProfile), typeof(ContentMappingProfile), typeof(IdentityMappingProfile), typeof(AuthMappingProfile));
            services.ConfigureApplicationServices(configuration);
            services.AddAuthentication(IdentitySettings.APP_SESSION_SCHEMA).AddCookie(IdentitySettings.APP_SESSION_SCHEMA);
            services.ConfigureBusinessServices();

            services.AddTransient<IInstallProvider, InstallProvider>();
            services.AddSingleton<InstallSettings>();

            return services;
        }
    }
}
