using Api.Content.GraphQLTypes;
using Api.Content.GraphQLTypes.InputTypes;
using Api.Content.Infrastructure.AutoMap;
using Api.Content.Resolvers;
using Api.Content.Resolvers.Contracts;
using AutoMapper;
using Coco.Business;
using Coco.Business.AutoMap;
using Coco.Framework.GraphQLTypes.ResultTypes;
using Coco.Framework.Infrastructure.Extensions;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Content.Infrastructure.Extensions
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

        public static IServiceCollection ConfigureContentServices(this IServiceCollection services)
        {
            services.ConfigureApplicationMapping();
            services.AddAutoMapper(typeof(IdentityMappingProfile), typeof(ContentMappingProfile));
            services.ConfigureApplicationServices();
            services.ConfigureBusinessServices();

            services.ConfigureGraphQlServices();

            return services;
        }
    }
}
