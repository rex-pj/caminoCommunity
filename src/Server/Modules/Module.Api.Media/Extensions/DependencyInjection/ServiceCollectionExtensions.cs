using Module.Api.Media.GraphQL.Resolvers;
using Module.Api.Media.GraphQL.Resolvers.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Module.Api.Media.GraphQL.Mutations;

namespace Module.Api.Media.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureGraphQlServices(this IServiceCollection services)
        {
            services.AddScoped<IImageResolver, ImageResolver>();
            services.AddScoped<IUserPhotoResolver, UserPhotoResolver>();

            services.AddGraphQLServer()
                .AddType<ImageMutations>()
                .AddType<UserPhotoMutations>();
            return services;
        }

        public static IServiceCollection ConfigureContentServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureGraphQlServices();
            return services;
        }
    }
}
