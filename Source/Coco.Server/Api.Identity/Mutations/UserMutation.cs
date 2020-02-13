using Api.Identity.GraphQLTypes.InputTypes;
using Api.Identity.GraphQLTypes.ResultTypes;
using Coco.Api.Framework.GraphQLTypes.ResultTypes;
using GraphQL.Types;
using Coco.Api.Framework.Models;
using Coco.Entities.Dtos.General;
using Coco.Entities.Dtos.User;
using Api.Identity.Resolvers.Contracts;

namespace Api.Identity.Mutations
{
    public class UserMutation : ObjectGraphType
    {
        public UserMutation(IUserResolver userResolver)
        {
            FieldAsync(typeof(ApiResultType<UpdatePerItemModel, ItemUpdatedResultType>), "updateUserInfoItem",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<UpdatePerItemInputType>> { Name = "criterias" }),
                resolve: async context => await userResolver.UpdateUserInfoItemAsync(context));

            FieldAsync(typeof(ApiResultType<UpdateUserPhotoDto, UserPhotoUpdatedResultType>), "updateAvatar",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<UpdateUserPhotoInputType>> { Name = "criterias" }),
                resolve: async context => await userResolver.UpdateAvatarAsync(context));

            FieldAsync(typeof(ApiResultType<UpdateUserPhotoDto, UserPhotoUpdatedResultType>), "updateUserCover",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<UpdateUserPhotoInputType>> { Name = "criterias" }),
                resolve: async context => await userResolver.UpdateCoverAsync(context));

            FieldAsync(typeof(ApiResultType<UpdateUserPhotoDto, UserPhotoDeletedResultType>), "deleteAvatar",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<DeleteUserPhotoInputType>> { Name = "criterias" }),
                resolve: async context => await userResolver.DeleteAvatarAsync(context));

            FieldAsync(typeof(ApiResultType<UpdateUserPhotoDto, UserPhotoDeletedResultType>), "deleteCover",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<DeleteUserPhotoInputType>> { Name = "criterias" }),
                resolve: async context => await userResolver.DeleteCoverAsync(context));

            FieldAsync(typeof(ApiResultType<UserIdentifierUpdateDto, UserIdentifierUpdateResultType>), "updateUserIdentifier",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<UserIdentifierUpdateInputType>> { Name = "user" }),
                resolve: async context => await userResolver.UpdateIdentifierAsync(context));

            FieldAsync<ApiResultType<UserTokenResult, UserTokenResultType>>("updatePassword",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<UserPasswordUpdateInputType>> { Name = "criterias" }),
                resolve: async context => await userResolver.UpdatePasswordAsync(context));
        }
    }
}
