using Api.Identity.GraphQLTypes.InputTypes;
using Api.Identity.Resolvers.Contracts;
using HotChocolate.Types;

namespace Api.Identity.MutationTypes
{
    //[AuthenticationUser]
    public class UserMutationType : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Field("updateUserInfoItem")
                .Argument("criterias", a => a.Type<NonNullType<UpdatePerItemInputType>>())
                .Resolver(async ctx => await ctx.Service<IUserResolver>().UpdateUserInfoItemAsync(ctx));

            descriptor.Field("updateAvatar")
                .Argument("criterias", a => a.Type<NonNullType<UpdateUserPhotoInputType>>())
                .Resolver(async ctx => await ctx.Service<IUserResolver>().UpdateAvatarAsync(ctx));

            descriptor.Field("updateUserCover")
                .Argument("criterias", a => a.Type<NonNullType<UpdateUserPhotoInputType>>())
                .Resolver(async ctx => await ctx.Service<IUserResolver>().UpdateCoverAsync(ctx));

            descriptor.Field("deleteAvatar")
                .Argument("criterias", a => a.Type<NonNullType<DeleteUserPhotoInputType>>())
                .Resolver(async ctx => await ctx.Service<IUserResolver>().DeleteAvatarAsync(ctx));

            descriptor.Field("deleteCover")
                .Argument("criterias", a => a.Type<NonNullType<DeleteUserPhotoInputType>>())
                .Resolver(async ctx => await ctx.Service<IUserResolver>().DeleteCoverAsync(ctx));

            descriptor.Field("updateUserIdentifier")
                .Argument("user", a => a.Type<NonNullType<UserIdentifierUpdateInputType>>())
                .Resolver(async ctx => await ctx.Service<IUserResolver>().UpdateIdentifierAsync(ctx));

            descriptor.Field("updatePassword")
                .Argument("criterias", a => a.Type<NonNullType<UserPasswordUpdateInputType>>())
                .Resolver(async ctx => await ctx.Service<IUserResolver>().UpdatePasswordAsync(ctx));
        }
    }
}
