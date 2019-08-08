using Api.Identity.Resolvers;
using Api.Identity.GraphQLTypes.InputTypes;
using Api.Identity.GraphQLTypes.ResultTypes;
using Coco.Api.Framework.GraphQLTypes.ResultTypes;
using Coco.Api.Framework.Models;
using GraphQL.Types;
using Coco.Entities.Model.General;
using Coco.Entities.Model.User;

namespace Api.Identity.Mutations
{
    public class UserMutation : ObjectGraphType
    {
        public UserMutation(UserResolver userResolver)
        {
            FieldAsync(typeof(ApiResultType<ItemUpdatedResultType, UpdatePerItemModel>), "updateUserInfoItem",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<UpdatePerItemInputType>> { Name = "criterias" }),
                resolve: async context => await userResolver.UpdateUserInfoItemAsync(context));

            FieldAsync(typeof(ApiResultType<UserPhotoUpdatedResultType, UpdateUserPhotoModel>), "updateAvatar",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<UpdateUserPhotoInputType>> { Name = "criterias" }),
                resolve: async context => await userResolver.UpdateAvatarAsync(context));

            FieldAsync(typeof(ApiResultType<UserPhotoUpdatedResultType, UpdateUserPhotoModel>), "updateUserCover",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<UpdateUserPhotoInputType>> { Name = "criterias" }),
                resolve: async context => await userResolver.UpdateCoverAsync(context));

            FieldAsync(typeof(ApiResultType<UserPhotoDeletedResultType, UpdateUserPhotoModel>), "deleteAvatar",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<DeleteUserPhotoInputType>> { Name = "criterias" }),
                resolve: async context => await userResolver.DeleteAvatarAsync(context));

            FieldAsync(typeof(ApiResultType<UserPhotoDeletedResultType, UpdateUserPhotoModel>), "deleteCover",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<DeleteUserPhotoInputType>> { Name = "criterias" }),
                resolve: async context => await userResolver.DeleteCoverAsync(context));

            FieldAsync(typeof(ApiResultType<UserProfileUpdateResultType, UserProfileUpdateModel>), "updateUserProfile",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<UserProfileUpdateInputType>> { Name = "user" }),
                resolve: async context => await userResolver.UpdateUserProfileAsync(context));

            FieldAsync(typeof(ApiResultType), "updatePassword",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<UserPasswordUpdateInputType>> { Name = "criterias" }),
                resolve: async context => await userResolver.UpdatePasswordAsync(context));
        }
    }
}
