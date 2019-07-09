using Api.Identity.Resolvers;
using Api.Identity.GraphQLTypes.ResultTypes;
using GraphQL.Types;
using Coco.Api.Framework.AccountIdentity;

namespace Api.Identity.Queries
{
    public class AccountQuery : ObjectGraphType
    {
        public AccountQuery(AccountResolver accountResolver)
        {
            Field<UserInfoResultType>("loggedUser",
                resolve: context => accountResolver.GetLoggedUser(context.UserContext as WorkContext));
        }
    }
}
