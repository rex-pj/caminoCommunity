using Api.Public.GraphQLTypes.InputTypes;
using Api.Public.GraphQLTypes.ResultTypes;
using Api.Public.Resolvers;
using Coco.Api.Framework.GraphQLTypes.ResultTypes;
using Coco.Api.Framework.Models;
using GraphQL.Types;

namespace Api.Public.Mutations
{
    public class UserMutation : ObjectGraphType
    {
        public UserMutation(UserResolver userResolver)
        {
            FieldAsync(typeof(RegisterResultType), "adduser",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<RegisterInputType>> { Name = "user" }),
                resolve: async context => await userResolver.SignupAsync(context));

            FieldAsync<ApiResultType<UserTokenResultType, UserTokenResult>>("signin",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<SigninInputType>> { Name = "signinModel" }),
                resolve: async context => await userResolver.SigninAsync(context));

            FieldAsync<ApiResultType>("forgotPassword",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<ForgotPasswordInputType>> { Name = "criterias" }),
                resolve: async context => await userResolver.ForgotPasswordAsync(context));
        }
    }
}
