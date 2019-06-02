using Coco.Api.Framework.Models;
using GraphQL.Types;

namespace Api.Auth.GraphQLTypes.ResultTypes
{
    public class UserInfoResultType: ObjectGraphType<UserInfo>
    {
        public UserInfoResultType()
        {
            Field(x => x.Lastname, type: typeof(StringGraphType));
            Field(x => x.Firstname, type: typeof(StringGraphType));
            Field(x => x.Email, type: typeof(StringGraphType));
            Field(x => x.DisplayName, type: typeof(StringGraphType));
            Field(x => x.IsActived, type: typeof(BooleanGraphType));
            Field(x => x.UserHashedId, type: typeof(StringGraphType));
        }
    }
}
