using Coco.Contract;
using Coco.Contract.MapBuilder;
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
            var fluentBuilder = MappingSchemaBuilder.FluentMappingBuilder;
            MappingSchemaBuilder.ApplyMappingBuilder(new ArticleCategoryMap(fluentBuilder));
            MappingSchemaBuilder.ApplyMappingBuilder(new UserPhotoMap(fluentBuilder));
            MappingSchemaBuilder.ApplyMappingBuilder(new UserPhotoTypeMap(fluentBuilder));
        }
    }
}
