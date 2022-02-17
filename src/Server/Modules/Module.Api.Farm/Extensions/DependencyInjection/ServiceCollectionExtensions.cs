using Module.Api.Farm.GraphQL.Resolvers;
using Module.Api.Farm.GraphQL.Resolvers.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Module.Api.Farm.GraphQL.Queries;
using Module.Api.Farm.GraphQL.Mutations;

namespace Module.Api.Farm.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureGraphQlServices(this IServiceCollection services)
        {
            services.AddScoped<IFarmTypeResolver, FarmTypeResolver>();
            services.AddScoped<IFarmResolver, FarmResolver>();

            services.AddGraphQLServer()
                .AddType<FarmQueries>()
                .AddType<FarmMutations>()
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
