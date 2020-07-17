using Coco.Framework.Models;
using HotChocolate.Types;

namespace  Coco.Api.Auth.GraphQLTypes.InputTypes
{
    public class ResetPasswordInputType : InputObjectType<ResetPasswordModel>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ResetPasswordModel> descriptor)
        {
            descriptor.Field(x => x.Email).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.ConfirmPassword).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.CurrentPassword).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.Password).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.Key).Type<NonNullType<StringType>>();
        }
    }
}
