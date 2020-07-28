using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Module.Web.ArticleManagement.Infrastructure.AutoMap;

namespace Module.Web.ArticleManagement.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureFileServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ArticleMappingProfile));

            return services;
        }
    }
}
