using Camino.Data.Contracts;
using Camino.Data.MapBuilders;
using Camino.DAL.Contracts;
using Camino.DAL.Mapping;

namespace Camino.DAL.Implementations
{
    public class ContentDataProvider : BaseDataProvider<ContentMappingSchema>, IContentDataProvider
    {
        public ContentDataProvider(ContentDbConnection dataConnection) : base(dataConnection)
        {

        }

        protected override void OnMappingSchemaCreating()
        {
            FluentMapBuilder.ApplyMappingBuilder<ArticleCategoryMap>()
                .ApplyMappingBuilder<UserPhotoMap>()
                .ApplyMappingBuilder<UserPhotoTypeMap>()
                .ApplyMappingBuilder<ArticleMap>()
                .ApplyMappingBuilder<PictureMap>()
                .ApplyMappingBuilder<ArticlePictureMap>();
        }
    }
}
