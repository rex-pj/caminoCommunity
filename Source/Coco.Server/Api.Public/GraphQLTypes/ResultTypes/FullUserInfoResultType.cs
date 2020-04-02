using Coco.Api.Framework.GraphQLTypes.ResultTypes;
using Coco.Api.Framework.Models;
using HotChocolate.Types;

namespace Api.Public.GraphQLTypes.ResultTypes
{
    public class ApiFullUserInfoResultType : ApiResultType<UserInfoExtend, FullUserInfoResultType>
    {

    }

    public class FullUserInfoResultType : ObjectType<UserInfoExtend>
    {
        protected override void Configure(IObjectTypeDescriptor<UserInfoExtend> descriptor)
        {
            descriptor.Field(x => x.Lastname).Type<StringType>();
            descriptor.Field(x => x.Firstname).Type<StringType>();
            descriptor.Field(x => x.Email).Type<StringType>();
            descriptor.Field(x => x.DisplayName).Type<StringType>();
            descriptor.Field(x => x.IsActived).Type<BooleanType>();
            descriptor.Field(x => x.UserIdentityId).Type<StringType>();
            descriptor.Field(x => x.Address).Type<StringType>();
            descriptor.Field(x => x.PhoneNumber).Type<StringType>();
            descriptor.Field(x => x.Description).Type<StringType>();
            descriptor.Field(x => x.BirthDate).Type<DateTimeType>();
            descriptor.Field(x => x.CreatedDate).Type<DateTimeType>();
            descriptor.Field(x => x.UpdatedDate).Type<DateTimeType>();
            descriptor.Field(x => x.GenderId).Type<IntType>();
            descriptor.Field(x => x.GenderLabel).Type<StringType>();
            descriptor.Field(x => x.CountryId).Type<ShortType>();
            descriptor.Field(x => x.CountryCode).Type<StringType>();
            descriptor.Field(x => x.CountryName).Type<StringType>();
            descriptor.Field(x => x.StatusId).Type<IntType>();
            descriptor.Field(x => x.StatusLabel).Type<StringType>();
            descriptor.Field(x => x.AvatarUrl).Type<StringType>();
            descriptor.Field(x => x.CoverPhotoUrl).Type<StringType>();
            descriptor.Field(x => x.GenderSelections)
                .Type<ListType<GenderSelectOptionType>>();
            descriptor.Field(x => x.CountrySelections)
                .Type<ListType<CountrySelectOptionType>>();
        }
    }
}
