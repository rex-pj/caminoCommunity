using AutoMapper;
using Coco.Framework.Infrastructure.AutoMap;
using Microsoft.Extensions.DependencyInjection;

namespace Coco.Framework.Infrastructure.Extensions
{
    public static class ApplicationMappingConfigure
    {
        public static void ConfigureApplicationMapping(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(FrameworkMappingProfile));
        }
    }
}
