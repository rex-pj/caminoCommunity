using Camino.Core.Constants;
using Camino.DAL.Entities;
using Camino.Data.MapBuilders;
using LinqToDB.Mapping;

namespace Camino.DAL.Mapping
{
    public class ProductCategoryProductMap : EntityMapBuilder<ProductCategoryProduct>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<ProductCategoryProduct>()
                .HasTableName(nameof(ProductCategoryProduct))
                .HasSchemaName(TableSchemaConst.DBO)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id)
                .Association(x => x.Product,
                    (productCategoryProduct, product) => productCategoryProduct.ProductId == product.Id)
                .Association(x => x.ProductCategory,
                    (productCategoryProduct, productCategory) => productCategoryProduct.ProductCategoryId == productCategory.Id);
        }
    }
}
