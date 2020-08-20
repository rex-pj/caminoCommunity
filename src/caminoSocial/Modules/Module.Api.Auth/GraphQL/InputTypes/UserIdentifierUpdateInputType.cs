using Camino.Service.Data.Request;
using HotChocolate.Types;

namespace Module.Api.Auth.GraphQL.InputTypes
{
    public class UserIdentifierUpdateInputType : InputObjectType<UserIdentifierUpdateRequest>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UserIdentifierUpdateRequest> descriptor)
        {
            descriptor.Field(x => x.Lastname).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.Firstname).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.DisplayName).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.Id).Ignore();
        }
    }
}
