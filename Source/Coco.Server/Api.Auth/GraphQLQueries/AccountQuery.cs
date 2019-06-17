using Api.Auth.GraphQLResolver;
using Api.Auth.GraphQLTypes.InputTypes;
using Api.Auth.GraphQLTypes.ResultTypes;
using GraphQL.Types;

namespace Api.Auth.GraphQLQueries
{
    public class AccountQuery : ObjectGraphType
    {

        public AccountQuery(AccountResolver accountResolver)
        {
            FieldAsync<SigninResultType>("signin",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<SigninInputType>> { Name = "signinModel" }),
                resolve: async context => await accountResolver.Signin(context));

            FieldAsync<UserInfoResultType>("loggedUser",
                resolve: async context => await accountResolver.GetLoggedUser(context));

            FieldAsync<FullUserInfoResultType>("fullUserInfo",
                arguments: new QueryArguments(new QueryArgument<FindUserInputType> { Name = "criterias" }),
                resolve: async context => await accountResolver.GetFullUserInfo(context));
        }
    }
}
