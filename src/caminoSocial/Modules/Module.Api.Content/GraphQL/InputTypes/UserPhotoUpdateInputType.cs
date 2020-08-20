using Camino.Service.Data.Request;
using HotChocolate.Types;

namespace Module.Api.Content.GraphQL.InputTypes
{
    public class UserPhotoUpdateInputType : InputObjectType<UserPhotoUpdateRequest>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UserPhotoUpdateRequest> descriptor)
        {
            descriptor.Field(x => x.ContentType).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.Height).Type<NonNullType<FloatType>>();
            descriptor.Field(x => x.Width).Type<NonNullType<FloatType>>();
            descriptor.Field(x => x.Scale).Type<NonNullType<FloatType>>();
            descriptor.Field(x => x.XAxis).Type<NonNullType<FloatType>>();
            descriptor.Field(x => x.YAxis).Type<NonNullType<FloatType>>();
            descriptor.Field(x => x.PhotoUrl).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.CanEdit).Type<NonNullType<BooleanType>>();
            descriptor.Field(x => x.FileName).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.UserPhotoType).Ignore();
        }
    }
}
