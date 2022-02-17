using Module.Api.Article.GraphQL.Resolvers;
using Module.Api.Article.GraphQL.Resolvers.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Module.Api.Article.GraphQL.Mutations;
using Module.Api.Article.GraphQL.Queries;

namespace Module.Api.Article.Extensions.DependencyInjection
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
