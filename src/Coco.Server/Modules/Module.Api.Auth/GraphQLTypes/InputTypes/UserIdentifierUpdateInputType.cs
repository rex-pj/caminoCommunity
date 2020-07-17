using Coco.Business.Dtos.Identity;
using HotChocolate.Types;

namespace  Module.Api.Auth.GraphQLTypes.InputTypes
{
    public class UserIdentifierUpdateInputType : InputObjectType<UserIdentifierUpdateDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UserIdentifierUpdateDto> descriptor)
        {
            descriptor.Field(x => x.Lastname).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.Firstname).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.DisplayName).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.Id).Ignore();
        }
    }
}
