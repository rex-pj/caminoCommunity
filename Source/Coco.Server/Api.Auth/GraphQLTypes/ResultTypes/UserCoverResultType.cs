using Api.Auth.Models;
using Api.Auth.Resolvers.Contracts;
using HotChocolate.Types;

namespace Api.Auth.GraphQLTypes.ResultTypes
{
    public class UserCoverResultType : ObjectType<UserCoverModel>
    {
        protected override void Configure(IObjectTypeDescriptor<UserCoverModel> descriptor)
        {
            descriptor.Name("UserCover");
            descriptor.Field(x => x.Url)
                .Type<StringType>()
                .Resolver(async ctx => await ctx.Service<IUserPhotoResolver>().GetCoverUrlByUserId(ctx));
        }
    }
}
