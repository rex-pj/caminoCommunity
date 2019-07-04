using Coco.Api.Framework.Models;
using GraphQL.Types;

namespace Api.Identity.GraphQLTypes.ResultTypes
{
    public class SigninResultType : ObjectGraphType<LoginResult>
    {
        public SigninResultType()
        {
            Field(x => x.AuthenticatorToken, type: typeof(StringGraphType));
            Field(x => x.IsSuccess, type: typeof(BooleanGraphType));
            Field(x => x.UserInfo, type: typeof(UserInfoResultType));
        }
    }
}
