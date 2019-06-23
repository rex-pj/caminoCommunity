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
                resolve: async context => await accountResolver.SignupAsync(context));

            FieldAsync(typeof(IdentityResultType), "updateUserInfo",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<UserInfoUpdateInputType>> { Name = "userInfo" }),
                resolve: async context => await accountResolver.UpdateUserInfoAsync(context));

            FieldAsync(typeof(UpdatePerItemResultType), "updateUserInfoItem",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<UpdatePerItemInputType>> { Name = "criterias" }),
                resolve: async context => await accountResolver.UpdateUserInfoItemAsync(context));
        }
    }
}
