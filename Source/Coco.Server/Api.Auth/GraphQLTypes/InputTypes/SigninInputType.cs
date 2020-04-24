using Api.Auth.Models;
using HotChocolate.Types;

namespace Api.Auth.GraphQLTypes.InputTypes
{
    public class SigninInputType : InputObjectType<SigninModel>
    {
        protected override void Configure(IInputObjectTypeDescriptor<SigninModel> descriptor)
        {
            descriptor.Field(x => x.Password).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.Username).Type<NonNullType<StringType>>();
        }
    }
}
