using Api.Auth.GraphQLTypes;
using Api.Auth.GraphQLTypes.InputTypes;
using Api.Auth.Infrastructure.AutoMap;
using Api.Auth.Models;
using Api.Auth.Resolvers;
using Api.Auth.Resolvers.Contracts;
using AutoMapper;
using Coco.Business;
using Coco.Business.AutoMap;
using Coco.Framework.GraphQLTypes.ResultTypes;
using Coco.Framework.Infrastructure.AutoMap;
using Coco.Framework.Infrastructure.Extensions;
using Coco.Framework.Models;
using HotChocolate;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Api.Auth.Infrastructure.Extensions
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
            var cocoSettings = serviceProvider.GetRequiredService<IOptions<CocoSettings>>().Value;
            services.AddCors(options =>
            {
                options.AddPolicy(cocoSettings.MyAllowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins(cocoSettings.AllowOrigins)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            return services;
        }
    }
}
