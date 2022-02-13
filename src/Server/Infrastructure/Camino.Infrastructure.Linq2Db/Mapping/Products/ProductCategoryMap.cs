using LinqToDB.Mapping;
using Camino.Infrastructure.Linq2Db.MapBuilders;
using Camino.Core.Domain.Products;
using Camino.Shared.Constants;

namespace Camino.Infrastructure.Linq2Db.Mapping.Products
{
    public class ProductCategoryMap : EntityMapBuilder<ProductCategory>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<ProductCategory>()
                .HasTableName(nameof(ProductCategory))
                .HasSchemaName(TableSchemaConst.Dbo)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id)
                .Association(x => x.ParentCategory, 
                    (productCategory, parentCategory) => productCategory.ParentId == parentCategory.Id)
                .Association(x => x.ChildCategories, 
                    (productCategory, childCategories) => productCategory.Id == childCategories.ParentId);
        }
    }
}
