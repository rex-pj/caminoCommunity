using HotChocolate.Types;
using Module.Api.Feed.Models;

namespace Module.Api.Feed.GraphQL.ResultTypes
{
    public class FeedResultType : ObjectType<FeedModel>
    {
        protected override void Configure(IObjectTypeDescriptor<FeedModel> descriptor)
        {
            descriptor.Field(x => x.Id).Type<LongType>();
            descriptor.Field(x => x.Name).Type<StringType>();
            descriptor.Field(x => x.Description).Type<StringType>();
            descriptor.Field(x => x.Address).Type<StringType>();
            descriptor.Field(x => x.CreatedByIdentityId).Type<StringType>();
            descriptor.Field(x => x.Description).Type<StringType>();
            descriptor.Field(x => x.CreatedById).Type<LongType>();
            descriptor.Field(x => x.CreatedDate).Type<DateTimeType>();
            descriptor.Field(x => x.CreatedByName).Type<StringType>();
            descriptor.Field(x => x.Price).Type<IntType>();
            descriptor.Field(x => x.PictureId).Type<LongType>();
            descriptor.Field(x => x.FeedType).Type<IntType>();
            descriptor.Field(x => x.CreatedByPhotoCode).Type<StringType>();
        }
    }
}
