using Camino.Core.Domain.Products;
using Camino.Shared.Constants;
using Camino.Infrastructure.Linq2Db.MapBuilders;
using LinqToDB.Mapping;

namespace Camino.Infrastructure.Linq2Db.Mapping.Products
{
    public class ProductPriceMap : EntityMapBuilder<ProductPrice>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<ProductPrice>()
                .HasTableName(nameof(ProductPrice))
                .HasSchemaName(TableSchemaConst.Dbo)
                .HasPrimaryKey(x => x.Id)
                .HasIdentity(x => x.Id)
                .Association(x => x.Product,
                    (productPrice, product) => productPrice.ProductId == product.Id);
        }
    }
}
