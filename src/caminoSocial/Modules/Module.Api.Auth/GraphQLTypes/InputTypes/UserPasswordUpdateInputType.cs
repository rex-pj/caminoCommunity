using Camino.Business.Dtos.Identity;
using HotChocolate.Types;

namespace  Module.Api.Auth.GraphQLTypes.InputTypes
{
    public class UserPasswordUpdateInputType : InputObjectType<UserPasswordUpdateDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UserPasswordUpdateDto> descriptor)
        {
            descriptor.Field(x => x.UserId).Ignore();
            descriptor.Field(x => x.ConfirmPassword).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.NewPassword).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.CurrentPassword).Type<NonNullType<StringType>>();
        }
    }
}
