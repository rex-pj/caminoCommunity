using Coco.Api.Auth.Models;
using HotChocolate.Types;

namespace  Coco.Api.Auth.GraphQLTypes.ResultTypes
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
