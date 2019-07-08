using Api.Identity.Resolvers;
using Api.Identity.GraphQLTypes.InputTypes;
using Api.Identity.GraphQLTypes.ResultTypes;
using Coco.Api.Framework.GraphQLTypes.ResultTypes;
using Coco.Api.Framework.Models;
using GraphQL.Types;
using Coco.Api.Framework.AccountIdentity;

namespace Api.Identity.Queries
{
    public class AccountQuery : ObjectGraphType
    {
        public AccountQuery(AccountResolver accountResolver)
        {
            Field<UserInfoResultType>("loggedUser",
                resolve: context =>
                {
                    var userContext = context.UserContext as WorkContext;
                    return accountResolver.GetLoggedUser(userContext);
                });

            FieldAsync<ApiResultType<FullUserInfoResultType, UserInfoExt>>("fullUserInfo",
                arguments: new QueryArguments(new QueryArgument<FindUserInputType> { Name = "criterias" }),
                resolve: async context =>
                {
                    var userContext = context.UserContext as WorkContext;
                    return await accountResolver.GetFullUserInfoAsync(context);
                });
        }
    }
}
