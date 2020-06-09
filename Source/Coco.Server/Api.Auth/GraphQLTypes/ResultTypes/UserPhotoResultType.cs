using Api.Auth.Models;
using HotChocolate.Types;

namespace Api.Auth.GraphQLTypes.ResultTypes
{
    public class UserPhotoResultType : ObjectType<UserPhotoModel>
    {
        protected override void Configure(IObjectTypeDescriptor<UserPhotoModel> descriptor)
        {
            descriptor.Field(x => x.Code).Type<StringType>();
            descriptor.Field(x => x.Description).Type<StringType>();
            descriptor.Field(x => x.Id).Type<LongType>();
            descriptor.Field(x => x.Name).Type<StringType>();
            descriptor.Field(x => x.PhotoType).Type<UserPhotoTypeEnumResultType>();
            descriptor.Field(x => x.Url).Type<StringType>();
            descriptor.Field(x => x.UserId).Type<LongType>();
        }
    }
}
