using Coco.Core.Constants;
using Coco.Data.Entities.Content;
using LinqToDB.Mapping;
using Coco.Data.MapBuilders;

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
