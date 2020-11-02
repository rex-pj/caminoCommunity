using Module.Api.Auth.Infrastructure.AutoMap;
using Module.Api.Auth.Models;
using Module.Api.Auth.GraphQL.Resolvers;
using Module.Api.Auth.GraphQL.Resolvers.Contracts;
using AutoMapper;
using Camino.Service.AutoMap;
using Camino.Framework.Infrastructure.AutoMap;
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
            services.AddTransient<IUserResolver, UserResolver>();
            services.AddTransient<ICountryResolver, CountryResolver>();
            services.AddTransient<IGenderResolver, GenderResolver>();
            services.AddTransient<IUserPhotoResolver, UserPhotoResolver>();

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
            services.AddAutoMapper(typeof(FrameworkMappingProfile), typeof(IdentityMappingProfile), typeof(AuthMappingProfile));

            services.Configure<RegisterConfirmationSettings>(configuration.GetSection(RegisterConfirmationSettings.Name));
            services.Configure<ResetPasswordSettings>(configuration.GetSection(ResetPasswordSettings.Name));

            services.ConfigureGraphQlServices();

            return services;
        }
    }
}
