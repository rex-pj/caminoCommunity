using Module.Api.Auth.Models;
using HotChocolate.Types;

namespace  Module.Api.Auth.GraphQLTypes.InputTypes
{
    public class ActiveUserInputType : InputObjectType<ActiveUserModel>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ActiveUserModel> descriptor)
        {
            descriptor.Field(t => t.Email).Type<NonNullType<StringType>>();
            descriptor.Field(t => t.ActiveKey).Type<NonNullType<IdType>>();
        }
    }
}
