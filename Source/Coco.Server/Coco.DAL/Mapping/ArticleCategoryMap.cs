using Coco.Core.Constants;
using Coco.Core.Infrastructure.MapBuilders;
using Coco.Core.Entities.Content;
using LinqToDB.Mapping;

namespace Coco.DAL.Mapping
{
    public class ArticleCategoryMap : EntityMapBuilder<ArticleCategory>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<ArticleCategory>()
                .HasTableName(nameof(ArticleCategory))
                .HasSchemaName(TableSchemaConst.DBO)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id)
                .Association(x => x.ParentCategory, 
                    (articleCategory, parentCategory) => articleCategory.ParentId == parentCategory.Id)
                .Association(x => x.ChildCategories, 
                    (articleCategory, childCategories) => articleCategory.Id == childCategories.ParentId);
        }
    }
}
