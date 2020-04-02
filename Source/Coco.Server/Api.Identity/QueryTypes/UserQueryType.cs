using Api.Identity.Resolvers.Contracts;
using HotChocolate.Types;

namespace Api.Identity.QueryTypes
{
    public class UserQueryType : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Field("signout")
                .Resolver(ctx => ctx.Service<IUserResolver>().SignoutAsync(ctx.ContextData));
        }
    }
}
