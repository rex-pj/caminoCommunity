using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Module.Navigation.Api.GraphQL.Queries;
using Module.Navigation.Api.GraphQL.Resolvers;
using Module.Navigation.Api.GraphQL.Resolvers.Contracts;

namespace Module.Navigation.Api.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureGraphQlServices(this IServiceCollection services)
        {
            services.AddScoped<IShortcutResolver, ShortcutResolver>();

            services.AddGraphQLServer()
                .AddType<ShortcutQueries>();
            return services;
        }

        public static IServiceCollection ConfigureContentServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureGraphQlServices();
            return services;
        }
    }
}
