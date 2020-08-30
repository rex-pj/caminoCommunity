using Camino.Core.Constants;
using LinqToDB.Mapping;
using Camino.Data.MapBuilders;
using Camino.DAL.Entities;

namespace Camino.DAL.Mapping
{
    public class ArticlePictureMap : EntityMapBuilder<ArticlePicture>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<ArticlePicture>()
                .HasTableName(nameof(ArticlePicture))
                .HasSchemaName(TableSchemaConst.DBO)
                .HasPrimaryKey(x => new { 
                    x.ArticleId,
                    x.PictureId
                })
                .Association(x => x.Picture, 
                    (articlePicture, picture) => articlePicture.PictureId == picture.Id)
                .Association(x => x.Article, 
                    (articlePicture, article) => articlePicture.ArticleId == article.Id);
        }
    }
}
