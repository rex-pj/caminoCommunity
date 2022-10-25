using Module.Product.Api.GraphQL.Resolvers;
using Module.Product.Api.GraphQL.Resolvers.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Module.Product.Api.GraphQL.Mutations;
using Module.Product.Api.GraphQL.Queries;

namespace Module.Product.Api.Extensions.DependencyInjection
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
