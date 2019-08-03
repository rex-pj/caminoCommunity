using Api.Public.Mutations;
using Api.Public.Queries;
using GraphQL;
using GraphQL.Types;

namespace Api.Public.GraphQLSchemas
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
