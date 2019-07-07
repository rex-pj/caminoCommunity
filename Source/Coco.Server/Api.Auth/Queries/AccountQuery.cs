using Api.Identity.Resolvers;
using Api.Identity.GraphQLTypes.InputTypes;
using Api.Identity.GraphQLTypes.ResultTypes;
using Coco.Api.Framework.GraphQLTypes.ResultTypes;
using Coco.Api.Framework.Models;
using GraphQL.Types;
using Coco.Api.Framework.GraphQLTypes;

namespace Api.Identity.Queries
{
    public class AccountQuery : ObjectGraphType
    {
        public AccountQuery(AccountResolver accountResolver)
        {
            FieldAsync<UserInfoResultType>("loggedUser",
                resolve: async context =>
                {
                    var userContext = context.UserContext as GraphQLUserContext;
                    return await accountResolver.GetLoggedUserAsync(context);
                });

            FieldAsync<ApiResultType<FullUserInfoResultType, UserInfoExt>>("fullUserInfo",
                arguments: new QueryArguments(new QueryArgument<FindUserInputType> { Name = "criterias" }),
                resolve: async context =>
                {
                    var userContext = context.UserContext as GraphQLUserContext;
                    return await accountResolver.GetFullUserInfoAsync(context);
                });
        }
    }
}
