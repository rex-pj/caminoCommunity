using AutoMapper;
using Coco.Business;
using Coco.Business.AutoMap;
using Coco.Framework.Infrastructure.Extensions;
using Coco.Management.Infrastructure.AutoMap;
using Microsoft.Extensions.DependencyInjection;

namespace Coco.Management.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureManagementServices(this IServiceCollection services)
        {
            services.ConfigureApplicationMapping();
            services.AddAutoMapper(typeof(ContentMappingProfile), typeof(IdentityMappingProfile), typeof(AuthMappingProfile));

            services.ConfigureApplicationServices();
            services.ConfigureBusinessServices();

            return services;
        }
    }
}
