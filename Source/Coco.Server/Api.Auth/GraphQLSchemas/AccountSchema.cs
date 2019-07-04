using Api.Identity.Mutations;
using Api.Identity.Queries;
using GraphQL;
using GraphQL.Types;

namespace Api.Identity.GraphQLSchemas
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
