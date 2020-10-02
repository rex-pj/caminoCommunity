using Camino.Core.Constants;
using LinqToDB.Mapping;
using Camino.Data.MapBuilders;
using Camino.DAL.Entities;

namespace Camino.DAL.Mapping
{
    public class ProductCategoryMap : EntityMapBuilder<ProductCategory>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<ProductCategory>()
                .HasTableName(nameof(ProductCategory))
                .HasSchemaName(TableSchemaConst.DBO)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id)
                .Association(x => x.ParentCategory, 
                    (productCategory, parentCategory) => productCategory.ParentId == parentCategory.Id)
                .Association(x => x.ChildCategories, 
                    (productCategory, childCategories) => productCategory.Id == childCategories.ParentId);
        }
    }
}
