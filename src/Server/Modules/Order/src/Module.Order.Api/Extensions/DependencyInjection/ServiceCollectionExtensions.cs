using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Module.Order.Api.GraphQL.Mutations;
using Module.Order.Api.GraphQL.Resolvers;
using Module.Order.Api.GraphQL.Resolvers.Contracts;

namespace Module.Order.Api.Extensions.DependencyInjection
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
