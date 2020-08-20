using Camino.Framework.Models;
using HotChocolate.Types;

namespace Camino.Framework.GraphQL.ResultTypes
{
    public class UserTokenResultType : ObjectType<UserTokenModel>
    {
        protected override void Configure(IObjectTypeDescriptor<UserTokenModel> descriptor)
        {
            descriptor.Field(x => x.AuthenticationToken).Type<StringType>();
            descriptor.Field(x => x.IsSucceed).Type<BooleanType>();
            descriptor.Field(x => x.UserInfo).Type<UserInfoResultType>();
        }
    }
}
