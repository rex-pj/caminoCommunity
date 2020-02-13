using Coco.Api.Framework.GraphQLTypes.ResultTypes;
using Coco.Entities.Dtos.User;
using GraphQL.Types;

namespace Api.Identity.GraphQLTypes.ResultTypes
{
    public class ApiUserIdentifierUpdateResultType: ApiResultType<UserIdentifierUpdateDto, UserIdentifierUpdateResultType>
    {

    }

    public class UserIdentifierUpdateResultType : ObjectGraphType<UserIdentifierUpdateDto>
    {
        public UserIdentifierUpdateResultType()
        {
            Field(x => x.Lastname, false, typeof(StringGraphType));
            Field(x => x.Firstname, false, typeof(StringGraphType));
            Field(x => x.DisplayName, false, typeof(StringGraphType));
        }
    }
}
