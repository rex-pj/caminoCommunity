using Api.Identity.Resolvers;
using Api.Identity.GraphQLTypes.InputTypes;
using Api.Identity.GraphQLTypes.ResultTypes;
using Coco.Api.Framework.GraphQLTypes.ResultTypes;
using Coco.Api.Framework.Models;
using GraphQL.Types;
using Coco.Entities.Model.General;

namespace Api.Identity.Mutations
{
    public class AccountMutation : ObjectGraphType
    {
        public AccountMutation(AccountResolver accountResolver)
        {
            FieldAsync(typeof(ApiResultType<ItemUpdatedResultType, UpdatePerItemModel>), "updateUserInfoItem",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<UpdatePerItemInputType>> { Name = "criterias" }),
                resolve: async context => await accountResolver.UpdateUserInfoItemAsync(context));

            FieldAsync(typeof(ApiResultType<UserPhotoUpdatedResultType, UpdateUserPhotoModel>), "updateAvatar",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<UpdateUserPhotoInputType>> { Name = "criterias" }),
                resolve: async context => await accountResolver.UpdateAvatarAsync(context));

            FieldAsync(typeof(ApiResultType<UserPhotoUpdatedResultType, UpdateUserPhotoModel>), "updateUserCover",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<UpdateUserPhotoInputType>> { Name = "criterias" }),
                resolve: async context => await accountResolver.UpdateCoverAsync(context));

            FieldAsync(typeof(ApiResultType<UserPhotoDeletedResultType, UpdateUserPhotoModel>), "deleteAvatar",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<DeleteUserPhotoInputType>> { Name = "criterias" }),
                resolve: async context => await accountResolver.DeleteAvatarAsync(context));

            FieldAsync(typeof(ApiResultType<UserPhotoDeletedResultType, UpdateUserPhotoModel>), "deleteCover",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<DeleteUserPhotoInputType>> { Name = "criterias" }),
                resolve: async context => await accountResolver.DeleteCoverAsync(context));
        }
    }
}
