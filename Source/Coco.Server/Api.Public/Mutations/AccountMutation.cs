using Api.Public.GraphQLTypes.InputTypes;
using Api.Public.GraphQLTypes.ResultTypes;
using Api.Public.Resolvers;
using GraphQL.Types;

namespace Api.Public.Mutations
{
    public class AccountMutation : ObjectGraphType
    {
        public AccountMutation(AccountResolver accountResolver)
        {
            FieldAsync(typeof(RegisterResultType), "adduser",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<RegisterInputType>> { Name = "user" }),
                resolve: async context => await accountResolver.SignupAsync(context));
        }
    }
}
