using Api.Auth.Models;
using Api.Auth.Resolvers.Contracts;
using HotChocolate.Types;

namespace Api.Auth.GraphQLTypes.ResultTypes
{
    public class UserAvatarResultType : ObjectType<UserAvatarModel>
    {
        protected override void Configure(IObjectTypeDescriptor<UserAvatarModel> descriptor)
        {
            descriptor.Name("UserAvatar");
            descriptor.Field(x => x.Url)
                .Type<StringType>()
                .Resolver(async ctx => await ctx.Service<IUserPhotoResolver>().GetAvatarUrlByUserId(ctx));
        }
    }
}
