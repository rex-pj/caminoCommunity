using Camino.Service.Projections.Request;
using HotChocolate.Types;

namespace Module.Api.Auth.GraphQL.ResultTypes
{
    public class UserIdentifierUpdateResultType : ObjectType<UserIdentifierUpdateRequest>
    {
        protected override void Configure(IObjectTypeDescriptor<UserIdentifierUpdateRequest> descriptor)
        {
            descriptor.Field(x => x.Lastname).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.Firstname).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.DisplayName).Type<NonNullType<StringType>>();
        }
    }
}
