using Module.Article.Api.GraphQL.Resolvers;
using Module.Article.Api.GraphQL.Resolvers.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Module.Article.Api.GraphQL.Mutations;
using Module.Article.Api.GraphQL.Queries;

namespace Module.Article.Api.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureGraphQlServices(this IServiceCollection services)
        {
            services.AddScoped<IArticleCategoryResolver, ArticleCategoryResolver>();
            services.AddScoped<IArticleResolver, ArticleResolver>();

            services.AddGraphQLServer().AddType<ArticleMutations>()
                .AddType<ArticleCategoryMutations>()
                .AddType<ArticleQueries>();
            return services;
        }

        public static IServiceCollection ConfigureContentServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureGraphQlServices();
            return services;
        }
    }
}
