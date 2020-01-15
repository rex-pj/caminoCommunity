using GraphQL.Types;
using Coco.Api.Framework.GraphQLTypes.ResultTypes;
using Api.Identity.Resolvers.Contracts;

namespace Api.Identity.Queries
{
    public class UserQuery : ObjectGraphType
    {
        public UserQuery(IUserResolver userResolver)
        {
            FieldAsync<SignoutResultType>("signout",
                resolve: async context => { 
                    return await userResolver.SignoutAsync(context.UserContext); 
                });
        }
    }
}
