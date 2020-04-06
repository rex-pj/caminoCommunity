using Coco.Entities.Dtos.General;
using HotChocolate.Types;

namespace Api.Identity.GraphQLTypes.ResultTypes
{
    public class UserPhotoUpdatedResultType : ObjectType<UserPhotoUpdateDto>
    {
        protected override void Configure(IObjectTypeDescriptor<UserPhotoUpdateDto> descriptor)
        {
            descriptor.Field(x => x.ContentType).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.Height).Type<NonNullType<FloatType>>();
            descriptor.Field(x => x.Width).Type<NonNullType<FloatType>>();
            descriptor.Field(x => x.XAxis).Type<NonNullType<FloatType>>();
            descriptor.Field(x => x.YAxis).Type<NonNullType<FloatType>>();
            descriptor.Field(x => x.PhotoUrl).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.CanEdit).Type<NonNullType<BooleanType>>();
        }
    }
}
