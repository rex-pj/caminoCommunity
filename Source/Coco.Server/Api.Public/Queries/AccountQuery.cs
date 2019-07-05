using Api.Public.Resolvers;
using GraphQL.Types;

namespace Api.Public.Queries
{
    public class AccountQuery : ObjectGraphType
    {
        public AccountQuery(AccountResolver accountResolver)
        {
            
        }
    }
}
