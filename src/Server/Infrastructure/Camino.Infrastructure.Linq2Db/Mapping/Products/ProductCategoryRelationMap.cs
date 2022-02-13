using Camino.Core.Domain.Products;
using Camino.Shared.Constants;
using Camino.Infrastructure.Linq2Db.MapBuilders;
using LinqToDB.Mapping;

namespace Camino.Infrastructure.Linq2Db.Mapping.Products
{
    public class ProductCategoryRelationMap : EntityMapBuilder<ProductCategoryRelation>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<ProductCategoryRelation>()
                .HasTableName(nameof(ProductCategoryRelation))
                .HasSchemaName(TableSchemaConst.Dbo)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id)
                .Association(x => x.Product,
                    (productCategoryRelation, product) => productCategoryRelation.ProductId == product.Id)
                .Association(x => x.ProductCategory,
                    (productCategoryRelation, productCategory) => productCategoryRelation.ProductCategoryId == productCategory.Id);
        }
    }
}
