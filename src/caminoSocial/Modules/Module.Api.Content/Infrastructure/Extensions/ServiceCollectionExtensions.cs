using Module.Api.Content.GraphQLTypes;
using Module.Api.Content.GraphQLTypes.InputTypes;
using Module.Api.Content.Infrastructure.AutoMap;
using Module.Api.Content.Resolvers;
using Module.Api.Content.Resolvers.Contracts;
using AutoMapper;
using Camino.Business;
using Camino.Business.AutoMap;
using Camino.Framework.GraphQLTypes.ResultTypes;
using Camino.Framework.Infrastructure.AutoMap;
using Camino.Framework.Infrastructure.Extensions;
using Camino.Framework.Models;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Module.Api.Content.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureGraphQlServices(this IServiceCollection services)
        {
            services.AddGraphQL(sp => SchemaBuilder.New()
                .AddServices(sp)
                .AddQueryType<QueryType>()
                .AddMutationType<MutationType>()
                .AddType<UserPhotoUpdateInputType>()
                .AddType<DeleteUserPhotoInputType>()
                .AddType(typeof(ListType<>))
                .AddType<AccessModeEnumType>()
                .AddType<CommonErrorType>()
                .Create());

            services.AddTransient<IImageResolver, ImageResolver>();
            services.AddTransient<IUserPhotoResolver, UserPhotoResolver>();

            return services;
        }

        public static IServiceCollection ConfigureContentServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(FrameworkMappingProfile), typeof(IdentityMappingProfile), typeof(ContentMappingProfile));
            services.ConfigureApplicationServices(configuration);
            services.ConfigureBusinessServices();
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
