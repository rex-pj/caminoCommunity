using Api.Public.GraphQLTypes.InputTypes;
using Api.Public.Resolvers.Contracts;
using Coco.Framework.GraphQLTypes.ResultTypes;
using HotChocolate.Types;

namespace Api.Public.QueryTypes
{
    public class UserQueryType : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Field<IUserResolver>(x => x.ActiveAsync(default))
                .Type<ApiResultType>()
                .Argument("criterias", a => a.Type<ActiveUserInputType>())
                .Resolver(ctx => ctx.Service<IUserResolver>().ActiveAsync(ctx));
        }
    }
}
