using HotChocolate.Types;
using Module.Api.Content.Models;

namespace Module.Api.Content.GraphQL.ResultTypes
{
    public class ArticleCreateResultType : ObjectType<ArticleModel>
    {
        protected override void Configure(IObjectTypeDescriptor<ArticleModel> descriptor)
        {
            descriptor.Field(x => x.Id).Type<LongType>();
            descriptor.Field(x => x.Name).Type<StringType>();
            descriptor.Field(x => x.Content).Type<StringType>();
            descriptor.Field(x => x.Thumbnail).Type<StringType>();
            descriptor.Field(x => x.ThumbnailFileName).Type<StringType>();
            descriptor.Field(x => x.ThumbnailFileType).Type<StringType>();
            descriptor.Field(x => x.ArticleCategoryId).Type<LongType>();
            descriptor.Field(x => x.Description).Ignore();
            descriptor.Field(x => x.ThumbnailId).Ignore();
            descriptor.Field(x => x.UpdateById).Ignore();
            descriptor.Field(x => x.UpdatedDate).Ignore();
            descriptor.Field(x => x.CreatedById).Ignore();
            descriptor.Field(x => x.CreatedDate).Ignore();
            descriptor.Field(x => x.CreatedBy).Ignore();
            descriptor.Field(x => x.UpdatedBy).Ignore();
            descriptor.Field(x => x.ArticleCategoryName).Ignore();
        }
    }
}
