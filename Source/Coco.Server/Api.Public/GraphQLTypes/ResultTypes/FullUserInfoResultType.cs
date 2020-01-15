using Coco.Api.Framework.GraphQLTypes.ResultTypes;
using Coco.Api.Framework.Models;
using GraphQL.Types;

namespace Api.Public.GraphQLTypes.ResultTypes
{
    public class ApiFullUserInfoResultType : ApiResultType<UserInfoExtend, FullUserInfoResultType>
    {

    }

    public class FullUserInfoResultType : ObjectGraphType<UserInfoExtend>
    {
        public FullUserInfoResultType()
        {
            Field(x => x.Lastname, type: typeof(StringGraphType));
            Field(x => x.Firstname, type: typeof(StringGraphType));
            Field(x => x.Email, type: typeof(StringGraphType));
            Field(x => x.DisplayName, type: typeof(StringGraphType));
            Field(x => x.IsActived, type: typeof(BooleanGraphType));
            Field(x => x.UserIdentityId, type: typeof(StringGraphType));
            Field(x => x.Address, type: typeof(StringGraphType));
            Field(x => x.PhoneNumber, type: typeof(StringGraphType));
            Field(x => x.Description, type: typeof(StringGraphType));
            Field(x => x.BirthDate, type: typeof(DateTimeGraphType));
            Field(x => x.CreatedDate, type: typeof(DateTimeGraphType));
            Field(x => x.UpdatedDate, type: typeof(DateTimeGraphType));
            Field(x => x.GenderId, type: typeof(IntGraphType));
            Field(x => x.GenderLabel, type: typeof(StringGraphType));
            Field(x => x.CountryId, type: typeof(ShortGraphType));
            Field(x => x.CountryCode, type: typeof(StringGraphType));
            Field(x => x.CountryName, type: typeof(StringGraphType));
            Field(x => x.StatusId, type: typeof(IntGraphType));
            Field(x => x.StatusLabel, type: typeof(StringGraphType));
            Field(x => x.AvatarUrl, type: typeof(StringGraphType));
            Field(x => x.CoverPhotoUrl, type: typeof(StringGraphType));
            Field(x => x.GenderSelections, type: typeof(ListGraphType<GenderSelectOptionType>));
            Field(x => x.CountrySelections, type: typeof(ListGraphType<CountrySelectOptionType>));
        }
    }
}
