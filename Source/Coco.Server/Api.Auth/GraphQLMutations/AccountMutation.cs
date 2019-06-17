using Api.Auth.GraphQLResolver;
using Api.Auth.GraphQLTypes.InputTypes;
using Api.Auth.GraphQLTypes.ResultTypes;
using GraphQL.Types;

namespace Api.Auth.GraphQLMutations
{
    public class AccountMutation : ObjectGraphType
    {
        public AccountMutation(AccountResolver accountResolver)
        {
            FieldAsync(typeof(RegisterResultType), "adduser",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<RegisterInputType>> { Name = "user" }),
                resolve: async context => await accountResolver.Signup(context));
        }
    }
}
