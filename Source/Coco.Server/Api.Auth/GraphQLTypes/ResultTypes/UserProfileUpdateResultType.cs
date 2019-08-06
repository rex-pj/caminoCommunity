using Coco.Entities.Model.User;
using GraphQL.Types;

namespace Api.Identity.GraphQLTypes.ResultTypes
{
    public class UserProfileUpdateResultType : ObjectGraphType<UserProfileUpdateModel>
    {
        public UserProfileUpdateResultType()
        {
            Field(x => x.Lastname, false, typeof(StringGraphType));
            Field(x => x.Firstname, false, typeof(StringGraphType));
            Field(x => x.DisplayName, false, typeof(StringGraphType));
        }
    }
}
