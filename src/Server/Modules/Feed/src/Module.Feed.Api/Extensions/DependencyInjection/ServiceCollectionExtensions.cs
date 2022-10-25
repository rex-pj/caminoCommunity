using Module.Feed.Api.GraphQL.Resolvers;
using Module.Feed.Api.GraphQL.Resolvers.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Module.Feed.Api.GraphQL.Queries;
using Module.Feed.Api.GraphQL.Mutations;
using Module.Feed.Api.Services;
using Module.Feed.Api.Services.Interfaces;

namespace Module.Feed.Api.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureGraphQlServices(this IServiceCollection services)
        {
            services.AddScoped<IFeedResolver, FeedResolver>();
            services.AddScoped<ISearchResolver, SearchResolver>();
            services.AddScoped<IFeedModelService, FeedModelService>();

            services.AddGraphQLServer()
                .AddType<FeedQueries>()
                .AddType<SearchQueries>();

            return services;
        }

        public static IServiceCollection ConfigureContentServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureGraphQlServices();
            return services;
        }
    }
}
