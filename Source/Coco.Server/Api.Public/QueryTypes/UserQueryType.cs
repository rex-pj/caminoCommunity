using Api.Public.GraphQLTypes.InputTypes;
using Api.Public.GraphQLTypes.ResultTypes;
using Api.Public.Resolvers.Contracts;
using Coco.Api.Framework.GraphQLTypes.ResultTypes;
using HotChocolate.Types;

namespace Api.Public.QueryTypes
{
    public class UserQueryType : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Field<IUserResolver>(x => x.GetLoggedUser(default))
                .Type<LoggedInResultType>()
                .Resolver(ctx => ctx.Service<IUserResolver>().GetLoggedUser(ctx));

            descriptor.Field<IUserResolver>(x => x.GetFullUserInfoAsync(default))
                .Type<FullUserInfoResultType>()
                .Argument("criterias", a => a.Type<FindUserInputType>())
                .Resolver(ctx => ctx.Service<IUserResolver>().GetFullUserInfoAsync(ctx));

            descriptor.Field<IUserResolver>(x => x.ActiveAsync(default))
                .Type<ApiResultType>()
                .Argument("criterias", a => a.Type<ActiveUserInputType>())
                .Resolver(ctx => ctx.Service<IUserResolver>().ActiveAsync(ctx));
        }
    }
}
