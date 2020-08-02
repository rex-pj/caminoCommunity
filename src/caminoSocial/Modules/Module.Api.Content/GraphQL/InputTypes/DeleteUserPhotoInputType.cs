using Module.Api.Content.Models;
using HotChocolate.Types;

namespace Module.Api.Content.GraphQL.InputTypes
{
    public class DeleteUserPhotoInputType : InputObjectType<PhotoDeleteModel>
    {
        protected override void Configure(IInputObjectTypeDescriptor<PhotoDeleteModel> descriptor)
        {
            descriptor.Field(x => x.CanEdit).Type<NonNullType<BooleanType>>();
        }
    }
}
