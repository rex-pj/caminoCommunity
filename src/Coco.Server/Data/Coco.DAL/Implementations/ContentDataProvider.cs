using Coco.Data.Contracts;
using Coco.Data.MapBuilders;
using Coco.DAL.Contracts;
using Coco.DAL.Mapping;

namespace Coco.DAL.Implementations
{
    public class ContentDataProvider : BaseDataProvider<ContentMappingSchema>, IContentDataProvider
    {
        public ContentDataProvider(ContentDbConnection dataConnection) : base(dataConnection)
        {

        }

        protected override void OnMappingSchemaCreating()
        {
            FluentMappingBuilder.ApplyMappingBuilder<ArticleCategoryMap>()
                .ApplyMappingBuilder<UserPhotoMap>()
                .ApplyMappingBuilder<UserPhotoTypeMap>();
        }
    }
}
