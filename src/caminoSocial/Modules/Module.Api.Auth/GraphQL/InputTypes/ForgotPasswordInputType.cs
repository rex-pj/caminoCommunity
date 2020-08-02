using Module.Api.Auth.Models;
using HotChocolate.Types;

namespace  Module.Api.Auth.GraphQL.InputTypes
{
    public class ForgotPasswordInputType : InputObjectType<ForgotPasswordModel>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ForgotPasswordModel> descriptor)
        {
            descriptor.Field(x => x.Email).Type<NonNullType<StringType>>();
        }
    }
}
