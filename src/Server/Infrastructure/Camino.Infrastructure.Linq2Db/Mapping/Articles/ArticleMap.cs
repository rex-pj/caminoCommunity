using Camino.Shared.Constants;
using LinqToDB.Mapping;
using Camino.Core.Domain.Articles;
using Camino.Infrastructure.Linq2Db.MapBuilders;

namespace Camino.Infrastructure.Linq2Db.Mapping.Articles
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
