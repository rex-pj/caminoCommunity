using Coco.Entities.Dtos.User;
using HotChocolate.Types;

namespace Api.Identity.GraphQLTypes.ResultTypes
{
    public class UserIdentifierUpdateResultType : ObjectType<UserIdentifierUpdateDto>
    {
        protected override void Configure(IObjectTypeDescriptor<UserIdentifierUpdateDto> descriptor)
        {
            descriptor.Field(x => x.Lastname).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.Firstname).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.DisplayName).Type<NonNullType<StringType>>();
        }
    }
}
