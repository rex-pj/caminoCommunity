using Api.Public.Mutations;
using Api.Public.Queries;
using GraphQL;
using GraphQL.Types;

namespace Api.Public.GraphQLSchemas
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
