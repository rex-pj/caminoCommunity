using Api.Identity.Mutations;
using Api.Identity.Queries;
using GraphQL;
using GraphQL.Types;

namespace Api.Identity.GraphQLSchemas
{
    public class UserSchema : Schema
    {
        public UserSchema(IDependencyResolver resolver)
        {
            Mutation = resolver.Resolve<UserMutation>();
            Query = resolver.Resolve<UserQuery>();
        }
    }
}
