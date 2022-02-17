using Module.Api.Feed.GraphQL.Resolvers;
using Module.Api.Feed.GraphQL.Resolvers.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Module.Api.Feed.GraphQL.Queries;
using Module.Api.Feed.GraphQL.Mutations;
using Module.Api.Feed.Services;
using Module.Api.Feed.Services.Interfaces;

namespace Module.Api.Feed.Extensions.DependencyInjection
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
