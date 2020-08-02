using Module.Api.Auth.Models;
using HotChocolate.Types;

namespace  Module.Api.Auth.GraphQL.ResultTypes
{
    public class UserAvatarResultType : ObjectType<UserAvatarModel>
    {
        protected override void Configure(IObjectTypeDescriptor<UserAvatarModel> descriptor)
        {
            descriptor.Name("UserAvatar");
            descriptor.Field(x => x.Url).Type<StringType>();
        }
    }
}
