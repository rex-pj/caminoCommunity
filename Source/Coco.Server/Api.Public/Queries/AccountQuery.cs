using Api.Public.GraphQLTypes.InputTypes;
using Api.Public.GraphQLTypes.ResultTypes;
using Api.Public.Resolvers;
using Coco.Api.Framework.GraphQLTypes.ResultTypes;
using Coco.Api.Framework.Models;
using GraphQL.Types;

namespace Api.Public.Queries
{
    public class AccountQuery : ObjectGraphType
    {
        public AccountQuery(AccountResolver accountResolver)
        {
            FieldAsync<ApiResultType<SigninResultType, LoginResult>>("signin",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<SigninInputType>> { Name = "signinModel" }),
                resolve: async context => await accountResolver.SigninAsync(context));
        }
    }
}
