using Coco.Api.Framework.Models;
using HotChocolate.Types;

namespace Coco.Api.Framework.GraphQLTypes.ResultTypes
{
    public class ApiUserTokenResultType : ApiResultType<UserTokenResult, UserTokenResultType>
    {

    }

    public class UserTokenResultType : ObjectType<UserTokenResult>
    {
        protected override void Configure(IObjectTypeDescriptor<UserTokenResult> descriptor)
        {
            descriptor.Field(x => x.AuthenticationToken).Type<StringType>();
            descriptor.Field(x => x.IsSucceed).Type<BooleanType>();
            descriptor.Field(x => x.UserInfo).Type<UserInfoResultType>();
        }
    }
}
