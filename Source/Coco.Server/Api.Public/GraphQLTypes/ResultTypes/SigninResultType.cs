using Coco.Api.Framework.Models;
using GraphQL.Types;

namespace Api.Public.GraphQLTypes.ResultTypes
{
    public class SigninResultType : ObjectGraphType<LoginResult>
    {
        public SigninResultType()
        {
            Field(x => x.AuthenticationToken, type: typeof(StringGraphType));
            Field(x => x.IsSuccess, type: typeof(BooleanGraphType));
            Field(x => x.UserInfo, type: typeof(UserInfoResultType));
        }
    }
}
