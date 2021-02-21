using Camino.Core.Constants;
using LinqToDB.Mapping;
using Camino.Infrastructure.MapBuilders;
using Camino.Core.Domain.Products;

namespace Camino.Infrastructure.Mapping.Products
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
