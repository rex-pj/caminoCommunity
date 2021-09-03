using Camino.Core.Domain.Products;
using Camino.Infrastructure.Commons.Constants;
using Camino.Infrastructure.MapBuilders;
using LinqToDB.Mapping;

namespace Camino.Infrastructure.Mapping.Products
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
