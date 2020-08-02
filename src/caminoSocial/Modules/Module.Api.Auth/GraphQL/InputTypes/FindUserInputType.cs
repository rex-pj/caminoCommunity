using Module.Api.Auth.Models;
using HotChocolate.Types;

namespace  Module.Api.Auth.GraphQL.InputTypes
{
    public class FindUserInputType : InputObjectType<FindUserModel>
    {
        protected override void Configure(IInputObjectTypeDescriptor<FindUserModel> descriptor)
        {
            descriptor.Field(x => x.UserId).Type<NonNullType<StringType>>();
        }
    }
}
