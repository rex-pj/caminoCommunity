using Api.Identity.GraphQLTypes.InputTypes;
using Api.Identity.GraphQLTypes.ResultTypes;
using Api.Identity.Resolvers.Contracts;
using Coco.Api.Framework.GraphQLTypes.ResultTypes;
using Coco.Api.Framework.Middlewares;
using HotChocolate.Types;

namespace Api.Identity.QueryTypes
{
    public class UserQueryType : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Field<IUserResolver>(x => x.SignoutAsync(default))
                .Type<ApiResultType>()
                .Directive<AuthenticationDirectiveType>()
                .Resolver(ctx => ctx.Service<IUserResolver>().SignoutAsync(ctx));

            descriptor.Field<IUserResolver>(x => x.GetLoggedUser(default))
                .Type<LoggedInResultType>()
                .Directive<InitializeSessionDirectiveType>()
                .Resolver(ctx => ctx.Service<IUserResolver>().GetLoggedUser(ctx));

            descriptor.Field<IUserResolver>(x => x.GetFullUserInfoAsync(default))
                .Type<FullUserInfoResultType>()
                .Directive<InitializeSessionDirectiveType>()
                .Argument("criterias", a => a.Type<FindUserInputType>())
                .Resolver(ctx => ctx.Service<IUserResolver>().GetFullUserInfoAsync(ctx));
        }
    }
}
