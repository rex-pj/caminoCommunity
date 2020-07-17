using Coco.Api.Auth.Models;
using HotChocolate.Types;

namespace  Coco.Api.Auth.GraphQLTypes.InputTypes
{
    public class ForgotPasswordInputType : InputObjectType<ForgotPasswordModel>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ForgotPasswordModel> descriptor)
        {
            descriptor.Field(x => x.Email).Type<NonNullType<StringType>>();
        }
    }
}
