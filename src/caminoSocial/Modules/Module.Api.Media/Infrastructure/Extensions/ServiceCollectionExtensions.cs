using Module.Api.Media.Infrastructure.AutoMap;
using Module.Api.Media.GraphQL.Resolvers;
using Module.Api.Media.GraphQL.Resolvers.Contracts;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Module.Api.Media.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureGraphQlServices(this IServiceCollection services)
        {
            services.AddTransient<IImageResolver, ImageResolver>();
            services.AddTransient<IUserPhotoResolver, UserPhotoResolver>();
            return services;
        }

        public static IServiceCollection ConfigureContentServices(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAutoMapper(typeof(ContentMappingProfile));
            services.ConfigureGraphQlServices();
            return services;
        }
    }
}
