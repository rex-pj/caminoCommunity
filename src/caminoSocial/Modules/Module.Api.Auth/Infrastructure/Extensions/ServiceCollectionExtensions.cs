using Module.Api.Auth.GraphQLTypes;
using Module.Api.Auth.GraphQLTypes.InputTypes;
using Module.Api.Auth.Infrastructure.AutoMap;
using Module.Api.Auth.Models;
using Module.Api.Auth.Resolvers;
using Module.Api.Auth.Resolvers.Contracts;
using AutoMapper;
using Camino.Business;
using Camino.Business.AutoMap;
using Camino.Framework.GraphQLTypes.ResultTypes;
using Camino.Framework.Infrastructure.AutoMap;
using Camino.Framework.Infrastructure.Extensions;
using Camino.Framework.Models;
using HotChocolate;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace  Module.Api.Auth.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureGraphQlServices(this IServiceCollection services)
        {
            services.AddAuthentication();
            services
                .AddGraphQL(sp => SchemaBuilder.New()
                .AddServices(sp)
                .AddQueryType<QueryType>()
                .AddMutationType<MutationType>()
                .AddType<CommonErrorType>()
                .AddType<UpdatePerItemInputType>()
                .AddType<SelectOptionType>()
                .AddType<FindUserInputType>()
                .AddType<UserPasswordUpdateInputType>()
                .AddType<UserIdentifierUpdateInputType>()
                .AddType<SignupInputType>()
                .AddType<ResetPasswordInputType>()
                .AddType<AccessModeEnumType>()
                .AddType<ActiveUserInputType>()
                .AddType<ForgotPasswordInputType>()
                .AddType<ICommonResult>()
                .Create());

            services.AddTransient<IUserResolver, UserResolver>();
            services.AddTransient<ICountryResolver, CountryResolver>();
            services.AddTransient<IGenderResolver, GenderResolver>();
            services.AddTransient<IUserPhotoResolver, UserPhotoResolver>();

            return services;
        }

        public static IServiceCollection ConfigureAuthServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(FrameworkMappingProfile), typeof(IdentityMappingProfile), typeof(AuthMappingProfile));
            services.ConfigureApplicationServices(configuration);

            services.Configure<RegisterConfirmationSettings>(configuration.GetSection(RegisterConfirmationSettings.Name));
            services.Configure<ResetPasswordSettings>(configuration.GetSection(ResetPasswordSettings.Name));
            services.ConfigureBusinessServices();

            services.AddHttpContextAccessor();
            services.ConfigureGraphQlServices();
            services.ConfigureAuthCorsServices(services.BuildServiceProvider());

            return services;
        }

        public static IServiceCollection ConfigureAuthCorsServices(this IServiceCollection services, IServiceProvider serviceProvider)
        {
            var appSettings = serviceProvider.GetRequiredService<IOptions<AppSettings>>().Value;
            services.AddCors(options =>
            {
                options.AddPolicy(appSettings.MyAllowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins(appSettings.AllowOrigins)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            return services;
        }
    }
}
