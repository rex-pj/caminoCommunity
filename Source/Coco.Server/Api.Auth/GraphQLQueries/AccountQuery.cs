using Api.Identity.GraphQLResolver;
using Api.Identity.GraphQLTypes.InputTypes;
using Api.Identity.GraphQLTypes.ResultTypes;
using Coco.Api.Framework.GraphQLTypes.ResultTypes;
using Coco.Api.Framework.Models;
using GraphQL.Types;

namespace Api.Identity.GraphQLQueries
{
    public class AccountQuery : ObjectGraphType
    {
        public AccountQuery(AccountResolver accountResolver)
        {
            FieldAsync<ApiResultType<SigninResultType, LoginResult>>("signin",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<SigninInputType>> { Name = "signinModel" }),
                resolve: async context => await accountResolver.SigninAsync(context));

            FieldAsync<UserInfoResultType>("loggedUser",
                resolve: async context => await accountResolver.GetLoggedUserAsync(context));

            FieldAsync<ApiResultType<FullUserInfoResultType, UserInfoExt>>("fullUserInfo",
                arguments: new QueryArguments(new QueryArgument<FindUserInputType> { Name = "criterias" }),
                resolve: async context => await accountResolver.GetFullUserInfoAsync(context));
        }
    }
}
