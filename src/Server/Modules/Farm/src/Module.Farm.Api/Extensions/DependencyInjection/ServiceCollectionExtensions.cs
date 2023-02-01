using Module.Farm.Api.GraphQL.Resolvers;
using Module.Farm.Api.GraphQL.Resolvers.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Module.Farm.Api.GraphQL.Queries;
using Module.Farm.Api.GraphQL.Mutations;

namespace Module.Farm.Api.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureGraphQlServices(this IServiceCollection services)
        {
            services.AddScoped<IFarmTypeResolver, FarmTypeResolver>();
            services.AddScoped<IFarmResolver, FarmResolver>();

            services.AddGraphQLServer()
                .AddType<FarmQueries>()
                .AddType<FarmTypeMutations>();

            return services;
        }

        public static IServiceCollection ConfigureContentServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureGraphQlServices();
            return services;
        }
    }
}
