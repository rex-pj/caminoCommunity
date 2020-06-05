using Api.Content.GraphQLTypes;
using Api.Content.GraphQLTypes.InputTypes;
using Coco.Framework.GraphQLTypes.ResultTypes;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Content.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGraphQlDependency(this IServiceCollection services)
        {
            return services.AddGraphQL(sp => SchemaBuilder.New()
                .AddServices(sp)
                .AddQueryType<QueryType>()
                .AddMutationType<MutationType>()
                .AddType<UserPhotoUpdateInputType>()
                .AddType<DeleteUserPhotoInputType>()
                .AddType(typeof(ListType<>))
                .AddType<AccessModeEnumType>()
                .AddType<CommonErrorType>()
                .Create());
        }
    }
}
