using Api.Public.Models;
using HotChocolate.Types;

namespace Api.Public.GraphQLTypes.InputTypes
{
    public class ForgotPasswordInputType : InputObjectType<ForgotPasswordModel>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ForgotPasswordModel> descriptor)
        {
            descriptor.Field(x => x.Email).Type<NonNullType<StringType>>();
        }
    }
}
