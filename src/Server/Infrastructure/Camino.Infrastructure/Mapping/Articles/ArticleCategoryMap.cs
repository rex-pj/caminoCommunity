using Camino.Core.Constants;
using LinqToDB.Mapping;
using Camino.Core.Domain.Articles;
using Camino.Infrastructure.MapBuilders;

namespace Camino.Infrastructure.Mapping.Articles
{
    public class ArticleCategoryMap : EntityMapBuilder<ArticleCategory>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<ArticleCategory>()
                .HasTableName(nameof(ArticleCategory))
                .HasSchemaName(TableSchemaConst.Dbo)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id)
                .Association(x => x.ParentCategory, 
                    (articleCategory, parentCategory) => articleCategory.ParentId == parentCategory.Id)
                .Association(x => x.ChildCategories, 
                    (articleCategory, childCategories) => articleCategory.Id == childCategories.ParentId);
        }
    }
}
