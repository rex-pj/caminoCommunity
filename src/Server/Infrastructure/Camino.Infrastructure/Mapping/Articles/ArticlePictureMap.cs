using Camino.Core.Constants;
using LinqToDB.Mapping;
using Camino.Infrastructure.MapBuilders;
using Camino.Core.Domain.Articles;

namespace Camino.Infrastructure.Mapping.Articles
{
    public class ArticlePictureMap : EntityMapBuilder<ArticlePicture>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<ArticlePicture>()
                .HasTableName(nameof(ArticlePicture))
                .HasSchemaName(TableSchemaConst.Dbo)
                .HasPrimaryKey(x => x.Id)
                .HasIdentity(x => x.Id)
                .Association(x => x.Picture,
                    (articlePicture, picture) => articlePicture.PictureId == picture.Id)
                .Association(x => x.Article,
                    (articlePicture, article) => articlePicture.ArticleId == article.Id);
        }
    }
}
