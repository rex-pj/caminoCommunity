using Module.Api.Auth.Infrastructure.AutoMap;
using Module.Api.Auth.Models;
using Module.Api.Auth.Resolvers;
using Module.Api.Auth.Resolvers.Contracts;
using AutoMapper;
using Camino.Business.AutoMap;
using Camino.Framework.Infrastructure.AutoMap;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
