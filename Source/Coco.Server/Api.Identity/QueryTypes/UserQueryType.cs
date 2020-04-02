using Api.Identity.Resolvers.Contracts;
using Coco.Api.Framework.GraphQLTypes.ResultTypes;
using HotChocolate.Types;

namespace Api.Identity.QueryTypes
{
    public class UserQueryType : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Field<IUserResolver>(x => x.SignoutAsync(default))
                .Type<ApiResultType>()
                .Resolver(ctx => ctx.Service<IUserResolver>().SignoutAsync(ctx));
        }
    }
}
