using Camino.Core.Constants;
using Camino.DAL.Entities;
using Camino.Data.MapBuilders;
using LinqToDB.Mapping;

namespace Camino.DAL.Mapping
{
    public class ProductCategoryRelationMap : EntityMapBuilder<ProductCategoryRelation>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<ProductCategoryRelation>()
                .HasTableName(nameof(ProductCategoryRelation))
                .HasSchemaName(TableSchemaConst.DBO)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id)
                .Association(x => x.Product,
                    (productCategoryRelation, product) => productCategoryRelation.ProductId == product.Id)
                .Association(x => x.ProductCategory,
                    (productCategoryRelation, productCategory) => productCategoryRelation.ProductCategoryId == productCategory.Id);
        }
    }
}
