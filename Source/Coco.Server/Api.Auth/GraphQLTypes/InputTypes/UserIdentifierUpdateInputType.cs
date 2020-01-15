using Coco.Entities.Dtos.User;
using GraphQL.Types;

namespace Api.Identity.GraphQLTypes.InputTypes
{
    public class UserIdentifierUpdateInputType : InputObjectGraphType<UserIdentifierUpdateDto>
    {
        public UserIdentifierUpdateInputType()
        {
            Field(x => x.Lastname, false, typeof(StringGraphType));
            Field(x => x.Firstname, false, typeof(StringGraphType));
            Field(x => x.DisplayName, false, typeof(StringGraphType));
            Field(x => x.UserIdentityId, type: typeof(StringGraphType));
            Field(x => x.AuthenticationToken, false, type: typeof(StringGraphType));
        }
    }
}
