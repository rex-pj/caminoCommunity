using Api.Auth.GraphQLMutations;
using GraphQL;
using GraphQL.Types;

namespace Api.Auth.GraphQLSchema
{
    public class AccountSchema : Schema
    {
        public AccountSchema(IDependencyResolver resolver)
        {
            Mutation = resolver.Resolve<AccountMutation>();
        }
    }
}
