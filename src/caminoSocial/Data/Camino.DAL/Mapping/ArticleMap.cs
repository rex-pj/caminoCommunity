using Camino.Core.Constants;
using Camino.Data.Entities.Content;
using Camino.Data.MapBuilders;
using LinqToDB.Mapping;

namespace Camino.DAL.Mapping
{
    public class ArticleMap : EntityMapBuilder<Article>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<Article>()
                .HasTableName(nameof(Article))
                .HasSchemaName(TableSchemaConst.DBO)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id)
                .Association(x => x.ArticleCategory,
                    (article, category) => article.ArticleCategoryId == category.Id);
        }
    }
}
