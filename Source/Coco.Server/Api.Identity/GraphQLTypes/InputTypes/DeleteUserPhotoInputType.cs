using Api.Identity.Models;
using HotChocolate.Types;

namespace Api.Identity.GraphQLTypes.InputTypes
{
    public class DeleteUserPhotoInputType : InputObjectType<PhotoDeleteModel>
    {
        protected override void Configure(IInputObjectTypeDescriptor<PhotoDeleteModel> descriptor)
        {
            descriptor.Field(x => x.CanEdit).Type<NonNullType<BooleanType>>();
        }
    }
}
