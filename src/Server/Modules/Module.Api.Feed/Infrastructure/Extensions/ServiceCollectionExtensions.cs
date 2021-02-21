using Module.Api.Feed.GraphQL.Resolvers;
using Module.Api.Feed.GraphQL.Resolvers.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Module.Api.Feed.GraphQL.Queries;

namespace Module.Api.Feed.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureGraphQlServices(this IServiceCollection services)
        {
            services.AddTransient<IFeedResolver, FeedResolver>();

            services.AddGraphQLServer()
                .AddType<FeedQueries>();

            return services;
        }

        public static IServiceCollection ConfigureContentServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureGraphQlServices();
            return services;
        }
    }
}
