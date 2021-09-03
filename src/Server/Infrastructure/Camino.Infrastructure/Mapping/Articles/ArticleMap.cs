using Camino.Infrastructure.Commons.Constants;
using LinqToDB.Mapping;
using Camino.Core.Domain.Articles;
using Camino.Infrastructure.MapBuilders;

namespace Camino.Infrastructure.Mapping.Articles
{
    public class ArticleMap : EntityMapBuilder<Article>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<Article>()
                .HasTableName(nameof(Article))
                .HasSchemaName(TableSchemaConst.Dbo)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id)
                .Association(x => x.ArticleCategory,
                    (article, category) => article.ArticleCategoryId == category.Id)
                .Association(x => x.ArticlePictures, (article, articlePicture) => article.Id == articlePicture.ArticleId);
        }
    }
}
