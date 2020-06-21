using Coco.Contract;
using Coco.Contract.MapBuilder;
using Coco.DAL.Contracts;
using Coco.DAL.Mapping;

namespace Coco.DAL.Implementations
{
    public class ContentDataProvider : CocoDataProvider, IContentDataProvider
    {
        public ContentDataProvider(ContentDbConnection dataConnection) : base(dataConnection)
        {

        }

        protected override void OnMappingSchemaCreating(MappingSchemaBuilder builder)
        {
            var fluentBuilder = builder.FluentMappingBuilder;
            builder.ApplyMappingBuilder(new ArticleCategoryMap(fluentBuilder));
            builder.ApplyMappingBuilder(new UserPhotoMap(fluentBuilder));
            builder.ApplyMappingBuilder(new UserPhotoTypeMap(fluentBuilder));
            base.OnMappingSchemaCreating(builder);
        }
    }
}
