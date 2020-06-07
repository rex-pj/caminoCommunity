using Coco.Framework.GraphQLTypes.ResultTypes;
using HotChocolate.Types;

namespace Api.Auth.GraphQLTypes.ResultTypes
{
    public class FullUserInfoResultType : UnionType
    {
        protected override void Configure(IUnionTypeDescriptor descriptor)
        {
            descriptor.Type<UserAvatarResultType>();
            descriptor.Type<UserCoverResultType>();
            descriptor.Type<GenderListType>();
            descriptor.Type<CountryListType>();
            descriptor.Type<UserInfoResultType>();
        }
    }
}
