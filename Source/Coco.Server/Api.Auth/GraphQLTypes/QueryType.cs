using Api.Auth.GraphQLTypes.InputTypes;
using Api.Auth.GraphQLTypes.ResultTypes;
using Api.Auth.Resolvers.Contracts;
using Coco.Framework.GraphQLTypes.ResultTypes;
using Coco.Framework.Middlewares;
using HotChocolate.Types;

namespace Api.Auth.GraphQLTypes
{
    public class QueryType : ObjectType
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

            // Public query
            descriptor.Field<IUserResolver>(x => x.ActiveAsync(default))
                .Type<ApiResultType>()
                .Argument("criterias", a => a.Type<ActiveUserInputType>())
                .Resolver(ctx => ctx.Service<IUserResolver>().ActiveAsync(ctx));
        }
    }
}
