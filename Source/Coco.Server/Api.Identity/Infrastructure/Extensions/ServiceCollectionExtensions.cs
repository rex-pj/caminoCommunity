using Api.Identity.GraphQLTypes.InputTypes;
using Api.Identity.GraphQLTypes.ResultTypes;
using Api.Identity.MutationTypes;
using Api.Identity.QueryTypes;
using Api.Identity.Resolvers;
using Api.Identity.Resolvers.Contracts;
using Coco.Api.Framework.GraphQLTypes.Redefines;
using Coco.Api.Framework.GraphQLTypes.ResultTypes;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Identity.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGraphQlDependency(this IServiceCollection services)
        {
            return services
                .AddTransient<IUserResolver, UserResolver>()
                .AddGraphQL(sp => SchemaBuilder.New()
                .AddServices(sp)
                .AddQueryType<UserQueryType>()
                .AddMutationType<UserMutationType>()
                .AddType(typeof(ListType<>))
                .AddType<AccessModeEnumType>()
                .AddType<ApiErrorType>()
                .AddType<DynamicType>()
                .AddType<ItemUpdatedResultType>()
                .AddType<UserPhotoUpdatedResultType>()
                .AddType<UserPhotoDeletedResultType>()
                .AddType<UserIdentifierUpdateResultType>()
                .AddType<UpdatePerItemInputType>()
                .AddType<DeleteUserPhotoInputType>()
                .AddType<UpdateUserPhotoInputType>()
                .AddType<UserPasswordUpdateInputType>()
                .AddType<UserIdentifierUpdateInputType>()
                .Create());
        }
    }
}
