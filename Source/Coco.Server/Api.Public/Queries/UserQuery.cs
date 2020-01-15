using Api.Public.GraphQLTypes.InputTypes;
using Api.Public.GraphQLTypes.ResultTypes;
using Api.Public.Resolvers.Contracts;
using Coco.Api.Framework.GraphQLTypes.ResultTypes;
using Coco.Api.Framework.Models;
using GraphQL.Types;

namespace Api.Public.Queries
{
    public class UserQuery : ObjectGraphType
    {
        public UserQuery(IUserResolver userResolver)
        {
            Field<LoggedInUserResultType>("loggedUser",
                resolve: context =>
                {
                    return userResolver.GetLoggedUser(context.UserContext);
                });

            FieldAsync<ApiResultType<UserInfoExtend, FullUserInfoResultType>>("fullUserInfo",
                arguments: new QueryArguments(new QueryArgument<FindUserInputType> { Name = "criterias" }),
                resolve: async context => {
                    return await userResolver.GetFullUserInfoAsync(context); 
                });

            FieldAsync<ActiveUserResultType>("active",
                arguments: new QueryArguments(new QueryArgument<ActiveUserInputType> { Name = "criterias" }),
                resolve: async context => {
                    return await userResolver.ActiveAsync(context);
                });
        }
    }
}
