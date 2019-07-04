using Api.Identity.GraphQLMutations;
using Api.Identity.GraphQLQueries;
using GraphQL;
using GraphQL.Types;

namespace Api.Identity.GraphQLSchema
{
    public class AccountSchema : Schema
    {
        public AccountSchema(IDependencyResolver resolver)
        {
            Mutation = resolver.Resolve<AccountMutation>();
            Query = resolver.Resolve<AccountQuery>();
        }
    }
}
