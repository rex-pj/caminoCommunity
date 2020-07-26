using Module.Api.Content.GraphQLTypes;
using Module.Api.Content.Infrastructure.AutoMap;
using Module.Api.Content.Resolvers;
using Module.Api.Content.Resolvers.Contracts;
using AutoMapper;
using Camino.Business.AutoMap;
using Camino.Framework.Infrastructure.AutoMap;
using HotChocolate;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Module.Api.Content.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureGraphQlServices(this IServiceCollection services)
        {
            //services.AddGraphQL(sp => SchemaBuilder.New()
            //    .AddServices(sp)
            //    .AddQueryType<QueryType>()
            //    .AddMutationType<MutationType>()
            //    .Create());

            //services.AddTransient<IImageResolver, ImageResolver>();
            //services.AddTransient<IUserPhotoResolver, UserPhotoResolver>();

            return services;
        }

        public static IServiceCollection ConfigureContentServices(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAutoMapper(typeof(FrameworkMappingProfile), typeof(IdentityMappingProfile), typeof(ContentMappingProfile));
            services.ConfigureGraphQlServices();
            return services;
        }
    }
}
