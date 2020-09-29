using HotChocolate.Types;
using Module.Api.Content.Models;

namespace Module.Api.Content.GraphQL.ResultTypes
{
    public class ArticleResultType : ObjectType<ArticleModel>
    {
        protected override void Configure(IObjectTypeDescriptor<ArticleModel> descriptor)
        {
            descriptor.Field(x => x.Id).Type<LongType>();
            descriptor.Field(x => x.Name).Type<StringType>();
            descriptor.Field(x => x.Content).Type<StringType>();
            descriptor.Field(x => x.Thumbnail).Type<StringType>();
            descriptor.Field(x => x.ThumbnailFileName).Type<StringType>();
            descriptor.Field(x => x.ThumbnailFileType).Type<StringType>();
            descriptor.Field(x => x.CreatedByIdentityId).Type<StringType>();
            descriptor.Field(x => x.ArticleCategoryId).Type<LongType>();
            descriptor.Field(x => x.Description).Type<StringType>();
            descriptor.Field(x => x.ThumbnailId).Type<LongType>();
            descriptor.Field(x => x.UpdateById).Type<LongType>();
            descriptor.Field(x => x.UpdatedDate).Type<DateTimeType>();
            descriptor.Field(x => x.CreatedById).Type<LongType>();
            descriptor.Field(x => x.CreatedDate).Type<DateTimeType>();
            descriptor.Field(x => x.CreatedBy).Type<StringType>();
            descriptor.Field(x => x.UpdatedBy).Type<StringType>();
            descriptor.Field(x => x.ArticleCategoryName).Type<StringType>();
            descriptor.Field(x => x.CreatedByPhotoCode).Type<StringType>();
        }
    }
}
