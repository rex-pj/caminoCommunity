using Coco.Api.Framework.Models;
using GraphQL.Types;

namespace Api.Identity.GraphQLTypes.ResultTypes
{
    public class UserInfoResultType : ObjectGraphType<UserInfo>
    {
        public UserInfoResultType()
        {
            Field(x => x.Lastname, type: typeof(StringGraphType));
            Field(x => x.Firstname, type: typeof(StringGraphType));
            Field(x => x.Email, type: typeof(StringGraphType));
            Field(x => x.DisplayName, type: typeof(StringGraphType));
            Field(x => x.IsActived, type: typeof(BooleanGraphType));
            Field(x => x.UserIdentityId, type: typeof(StringGraphType));

            Field(x => x.Address, type: typeof(StringGraphType));
            Field(x => x.BirthDate, type: typeof(DateTimeGraphType));
            Field(x => x.CountryId, type: typeof(IntGraphType));
            Field(x => x.CountryName, type: typeof(StringGraphType));
            Field(x => x.Description, type: typeof(StringGraphType));
            Field(x => x.CreatedDate, type: typeof(DateTimeGraphType));
            Field(x => x.PhoneNumber, type: typeof(StringGraphType));
            Field(x => x.AvatarUrl, type: typeof(StringGraphType));
            Field(x => x.CoverPhotoUrl, type: typeof(StringGraphType));
        }
    }
}
