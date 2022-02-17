using Module.Api.Product.GraphQL.Resolvers;
using Module.Api.Product.GraphQL.Resolvers.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Module.Api.Product.GraphQL.Mutations;
using Module.Api.Product.GraphQL.Queries;

namespace Module.Api.Product.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureGraphQlServices(this IServiceCollection services)
        {
            services.AddScoped<IProductCategoryResolver, ProductCategoryResolver>();
            services.AddScoped<IProductResolver, ProductResolver>();
            services.AddScoped<IProductAttributeResolver, ProductAttributeResolver>();

            services.AddGraphQLServer()
                .AddType<ProductQueries>()
                .AddType<ProductMutations>()
                .AddType<ProductCategoryMutations>()
                .AddType<ProductAttributeMutations>();

            return services;
        }

        public static IServiceCollection ConfigureContentServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureGraphQlServices();
            return services;
        }
    }
}
