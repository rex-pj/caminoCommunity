using Module.Api.Product.GraphQL.Resolvers;
using Module.Api.Product.GraphQL.Resolvers.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Module.Api.Product.GraphQL.Mutations;
using Module.Api.Product.GraphQL.Queries;

namespace Module.Api.Product.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureGraphQlServices(this IServiceCollection services)
        {
            services.AddTransient<IProductCategoryResolver, ProductCategoryResolver>();
            services.AddTransient<IProductResolver, ProductResolver>();

            services.AddGraphQLServer()
                .AddType<ProductQueries>()
                .AddType<ProductMutations>()
                .AddType<ProductCategoryMutations>();

            return services;
        }

        public static IServiceCollection ConfigureContentServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureGraphQlServices();
            return services;
        }
    }
}
