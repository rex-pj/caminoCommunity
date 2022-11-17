using Module.Auth.Api.Models;
using Module.Auth.Api.GraphQL.Resolvers;
using Module.Auth.Api.GraphQL.Resolvers.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Module.Auth.Api.GraphQL.Mutations;
using Module.Auth.Api.GraphQL.Queries;
using Camino.Infrastructure.DependencyInjection;
using System.Reflection;

namespace Module.Auth.Api.Extensions.DependencyInjection
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
                .AddType<AuthenticationQueries>()
                .AddType<UserMutations>();

            return services;
        }

        public static IServiceCollection ConfigureAuthServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RegisterConfirmationSettings>(configuration.GetSection(RegisterConfirmationSettings.Name));
            services.Configure<ResetPasswordSettings>(configuration.GetSection(ResetPasswordSettings.Name));

            services.ConfigureGraphQlServices();

            services.AddModuleDependencies(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
