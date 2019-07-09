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
            FieldAsync<ApiResultType<FullUserInfoResultType, UserInfoExt>>("fullUserInfo",
                arguments: new QueryArguments(new QueryArgument<FindUserInputType> { Name = "criterias" }),
                resolve: async context => await accountResolver.GetFullUserInfoAsync(context));
        }
    }
}
