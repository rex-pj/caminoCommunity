using Module.Api.Auth.Models;
using Module.Api.Auth.GraphQL.Resolvers;
using Module.Api.Auth.GraphQL.Resolvers.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Module.Api.Auth.GraphQL.Mutations;
using Module.Api.Auth.GraphQL.Queries;

namespace Module.Api.Auth.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureGraphQlServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticateResolver, AuthenticateResolver>();
            services.AddScoped<IUserResolver, UserResolver>();
            services.AddScoped<ICountryResolver, CountryResolver>();
            services.AddScoped<IGenderResolver, GenderResolver>();
            services.AddScoped<IUserPhotoResolver, UserPhotoResolver>();

            services.AddGraphQLServer()
                .AddType<CountryQueries>()
                .AddType<GenderQueries>()
                .AddType<UserPhotoQueries>()
                .AddType<UserQueries>()
                .AddType<UserMutations>();

            return services;
        }

        public static IServiceCollection ConfigureAuthServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RegisterConfirmationSettings>(configuration.GetSection(RegisterConfirmationSettings.Name));
            services.Configure<ResetPasswordSettings>(configuration.GetSection(ResetPasswordSettings.Name));

            services.ConfigureGraphQlServices();

            return services;
        }
    }
}
