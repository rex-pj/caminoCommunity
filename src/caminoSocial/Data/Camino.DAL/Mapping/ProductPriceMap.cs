using Camino.Core.Constants;
using Camino.DAL.Entities;
using Camino.Data.MapBuilders;
using LinqToDB.Mapping;

namespace Camino.DAL.Mapping
{
    public class ProductPriceMap : EntityMapBuilder<ProductPrice>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<ProductPrice>()
                .HasTableName(nameof(ProductPrice))
                .HasSchemaName(TableSchemaConst.DBO)
                .HasPrimaryKey(x => x.Id)
                .HasIdentity(x => x.Id)
                .Association(x => x.Product,
                    (productPrice, product) => productPrice.ProductId == product.Id);
        }
    }
}
