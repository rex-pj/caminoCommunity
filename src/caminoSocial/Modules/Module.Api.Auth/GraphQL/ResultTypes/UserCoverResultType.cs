using Module.Api.Auth.Models;
using HotChocolate.Types;

namespace  Module.Api.Auth.GraphQL.ResultTypes
{
    public class UserCoverResultType : ObjectType<UserCoverModel>
    {
        protected override void Configure(IObjectTypeDescriptor<UserCoverModel> descriptor)
        {
            descriptor.Name("UserCover");
            descriptor.Field(x => x.Url).Type<StringType>();
        }
    }
}
