using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Module.Api.Order.GraphQL.Mutations;
using Module.Api.Order.GraphQL.Resolvers;
using Module.Api.Order.GraphQL.Resolvers.Contracts;

namespace Module.Api.Order.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureGraphQlServices(this IServiceCollection services)
        {
            services.AddScoped<IOrderResolver, OrderResolver>();;

            services.AddGraphQLServer()
                .AddType<OrderMutations>();

            return services;
        }

        public static IServiceCollection ConfigureContentServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureGraphQlServices();
            return services;
        }
    }
}
