using Module.Api.Content.Infrastructure.AutoMap;
using Module.Api.Content.GraphQL.Resolvers;
using Module.Api.Content.GraphQL.Resolvers.Contracts;
using AutoMapper;
using Camino.Service.AutoMap;
using Camino.Framework.Infrastructure.AutoMap;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Module.Api.Content.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureGraphQlServices(this IServiceCollection services)
        {
            services.AddTransient<IImageResolver, ImageResolver>();
            services.AddTransient<IUserPhotoResolver, UserPhotoResolver>();
            services.AddTransient<IArticleCategoryResolver, ArticleCategoryResolver>();
            return services;
        }

        public static IServiceCollection ConfigureContentServices(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAutoMapper(typeof(FrameworkMappingProfile), typeof(IdentityMappingProfile), typeof(ContentMappingProfile));
            services.ConfigureGraphQlServices();
            return services;
        }
    }
}
