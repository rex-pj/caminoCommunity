using Coco.Entities.Dtos.General;
using HotChocolate.Types;

namespace Api.Identity.GraphQLTypes.InputTypes
{
    public class DeleteUserPhotoInputType : InputObjectType<UpdateUserPhotoDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateUserPhotoDto> descriptor)
        {
            descriptor.Field(x => x.CanEdit).Type<NonNullType<BooleanType>>();
        }
    }
}
