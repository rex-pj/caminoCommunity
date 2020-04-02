using Coco.Api.Framework.GraphQLTypes.ResultTypes;
using Coco.Entities.Dtos.General;
using HotChocolate.Types;

namespace Api.Identity.GraphQLTypes.ResultTypes
{
    public class ApiUserPhotoDeletedResultType: ApiResultType<UpdateUserPhotoDto, UserPhotoDeletedResultType>
    {

    }

    public class UserPhotoDeletedResultType : ObjectType<UpdateUserPhotoDto>
    {
        protected override void Configure(IObjectTypeDescriptor<UpdateUserPhotoDto> descriptor)
        {
            descriptor.Field(x => x.CanEdit).Type<BooleanType>();
        }
    }
}
