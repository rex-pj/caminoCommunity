using Module.Api.Auth.Models;
using HotChocolate.Types;

namespace  Module.Api.Auth.GraphQL.InputTypes
{
    public class SignupInputType : InputObjectType<SignupModel>
    {
        protected override void Configure(IInputObjectTypeDescriptor<SignupModel> descriptor)
        {
            descriptor.Field(x => x.Lastname).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.Firstname).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.Email).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.Password).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.ConfirmPassword).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.GenderId).Type<IntType>();
            descriptor.Field(x => x.BirthDate).Type<NonNullType<DateTimeType>>();
        }
    }
}
