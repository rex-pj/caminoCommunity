using Api.Identity.GraphQLSchemas;
using Api.Identity.GraphQLTypes.InputTypes;
using Api.Identity.GraphQLTypes.ResultTypes;
using Api.Identity.Mutations;
using Api.Identity.Queries;
using Api.Identity.Resolvers;
using Api.Identity.Resolvers.Contracts;
using Coco.Api.Framework.GraphQLTypes.Redefines;
using Coco.Api.Framework.GraphQLTypes.ResultTypes;
using Coco.Api.Framework.Models;
using Coco.Entities.Dtos.General;
using Coco.Entities.Dtos.User;
using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Identity.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGraphQlDependency(this IServiceCollection services)
        {
            return services
                .AddTransient<IUserResolver, UserResolver>()
                .AddSingleton<IDocumentExecuter, DocumentExecuter>()
                .AddTransient<UserMutation>()
                .AddTransient<UserQuery>()
                .AddSingleton(typeof(ListGraphType<>))
                .AddSingleton<AccessModeEnumType>()
                .AddSingleton<ApiErrorType>()
                .AddSingleton<DynamicGraphType>()
                .AddSingleton<ItemUpdatedResultType>()
                .AddSingleton<UserPhotoUpdatedResultType>()
                .AddSingleton<UserPhotoDeletedResultType>()
                .AddSingleton<UserIdentifierUpdateResultType>()
                .AddSingleton<UpdatePerItemInputType>()
                .AddSingleton<DeleteUserPhotoInputType>()
                .AddSingleton<UpdateUserPhotoInputType>()
                .AddSingleton<UserPasswordUpdateInputType>()
                .AddSingleton<UserIdentifierUpdateInputType>()
                .AddSingleton<ApiResultType<UpdatePerItemModel, ItemUpdatedResultType>, ApiItemUpdatedResultType>()
                .AddSingleton<ApiResultType<UpdateUserPhotoDto, UserPhotoUpdatedResultType>, ApiUserPhotoUpdatedResultType>()
                .AddSingleton<ApiResultType<UpdateUserPhotoDto, UserPhotoDeletedResultType>, ApiUserPhotoDeletedResultType>()
                .AddSingleton<ApiResultType<UserIdentifierUpdateDto, UserIdentifierUpdateResultType>, ApiUserIdentifierUpdateResultType>()
                .AddSingleton<ApiResultType<UserTokenResult, UserTokenResultType>, ApiUserTokenResultType>()
                .AddSingleton<SignoutResultType>()
                .AddSingleton<UserTokenResultType>()
                .AddSingleton<UserInfoResultType>()
                .AddScoped<ISchema, UserSchema>();
        }
    }
}
