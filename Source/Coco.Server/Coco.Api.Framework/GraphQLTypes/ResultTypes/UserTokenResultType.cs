using Coco.Api.Framework.Models;
using GraphQL.Types;

namespace Coco.Api.Framework.GraphQLTypes.ResultTypes
{
    public class UserTokenResultType : ObjectGraphType<UserTokenResult>
    {
        public UserTokenResultType()
        {
            Field(x => x.AuthenticationToken, type: typeof(StringGraphType));
            Field(x => x.IsSuccess, type: typeof(BooleanGraphType));
            Field(x => x.UserInfo, type: typeof(UserInfoResultType));
        }
    }
}
