using Api.Public.GraphQLTypes.InputTypes;
using Api.Public.GraphQLTypes.ResultTypes;
using Api.Public.Resolvers.Contracts;
using Coco.Api.Framework.GraphQLTypes.ResultTypes;
using Coco.Api.Framework.Models;
using GraphQL.Types;

namespace Api.Public.Mutations
{
    public class UserMutation : ObjectGraphType
    {
        public UserMutation(IUserResolver userResolver)
        {
            FieldAsync(typeof(RegisterResultType), "signup",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<RegisterInputType>> { Name = "user" }),
                resolve: async context => await userResolver.SignupAsync(context));

            FieldAsync<ApiResultType<UserTokenResult, UserTokenResultType>>("signin",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<SigninInputType>> { Name = "args" }),
                resolve: async context => await userResolver.SigninAsync(context));

            FieldAsync<ApiResultType>("forgotPassword",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<ForgotPasswordInputType>> { Name = "criterias" }),
                resolve: async context => await userResolver.ForgotPasswordAsync(context));

            FieldAsync<ResetPasswordResultType>("resetPassword",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<ResetPasswordInputType>> { Name = "criterias" }),
                resolve: async context => await userResolver.ResetPasswordAsync(context));
        }
    }
}
