using Coco.Api.Content.GraphQLTypes;
using Coco.Api.Content.GraphQLTypes.InputTypes;
using Coco.Api.Content.Infrastructure.AutoMap;
using Coco.Api.Content.Resolvers;
using Coco.Api.Content.Resolvers.Contracts;
using AutoMapper;
using Coco.Business;
using Coco.Business.AutoMap;
using Coco.Framework.GraphQLTypes.ResultTypes;
using Coco.Framework.Infrastructure.AutoMap;
using Coco.Framework.Infrastructure.Extensions;
using Coco.Framework.Models;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Coco.Api.Content.Infrastructure.Extensions
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
