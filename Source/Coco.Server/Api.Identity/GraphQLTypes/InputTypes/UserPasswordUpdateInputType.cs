using Coco.Entities.Dtos.User;
using HotChocolate.Types;

namespace Api.Identity.GraphQLTypes.InputTypes
{
    public class UserPasswordUpdateInputType : InputObjectType<UserPasswordUpdateDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UserPasswordUpdateDto> descriptor)
        {
            descriptor.Field(x => x.UserId).Type<NonNullType<FloatType>>();
            descriptor.Field(x => x.ConfirmPassword).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.NewPassword).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.CurrentPassword).Type<NonNullType<StringType>>();
        }
    }
}
