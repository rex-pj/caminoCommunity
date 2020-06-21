using Coco.Common.Const;
using Coco.Contract.MapBuilder;
using Coco.Entities.Domain.Content;
using LinqToDB.Mapping;

namespace Coco.DAL.Mapping
{
    public class ArticleCategoryMap : EntityTypeBuilder<ArticleCategory>
    {
        public ArticleCategoryMap(FluentMappingBuilder fluentMappingBuilder) : base(fluentMappingBuilder)
        {
        }

        public override void Configure(FluentMappingBuilder builder)
        {
            builder.Entity<ArticleCategory>()
                .HasTableName(nameof(ArticleCategory))
                .HasSchemaName(TableSchemaConst.DBO)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id)
                .Association(x => x.ParentCategory, 
                    (articleCategory, parentCategory) => articleCategory.ParentCategoryId == parentCategory.Id)
                .Association(x => x.ChildCategories, 
                    (articleCategory, childCategories) => articleCategory.Id == childCategories.ParentCategoryId);
        }
    }
}
