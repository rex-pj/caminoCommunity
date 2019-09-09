using Api.Identity.Resolvers;
using Api.Identity.GraphQLTypes.ResultTypes;
using GraphQL.Types;
using Coco.Api.Framework.SessionManager.Contracts;

namespace Api.Identity.Queries
{
    public class UserQuery : ObjectGraphType
    {
        public UserQuery(UserResolver userResolver)
        {
            Field<LoggedInUserResultType>("loggedUser",
                resolve: context => userResolver.GetLoggedUser(context.UserContext as ISessionContext));
        }
    }
}
