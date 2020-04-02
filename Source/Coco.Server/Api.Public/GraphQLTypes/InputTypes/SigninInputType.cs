using Api.Public.Models;
using HotChocolate.Types;

namespace Api.Public.GraphQLTypes.InputTypes
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
