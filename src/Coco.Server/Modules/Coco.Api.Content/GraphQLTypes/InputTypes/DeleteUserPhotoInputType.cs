using Coco.Api.Content.Models;
using HotChocolate.Types;

namespace Coco.Api.Content.GraphQLTypes.InputTypes
{
    public class DeleteUserPhotoInputType : InputObjectType<PhotoDeleteModel>
    {
        protected override void Configure(IInputObjectTypeDescriptor<PhotoDeleteModel> descriptor)
        {
            descriptor.Field(x => x.CanEdit).Type<NonNullType<BooleanType>>();
        }
    }
}
