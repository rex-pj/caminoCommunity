using LinqToDB.Mapping;
using Camino.Infrastructure.Linq2Db.MapBuilders;
using Camino.Core.Domain.Products;
using Camino.Shared.Constants;

namespace Camino.Infrastructure.Linq2Db.Mapping.Products
{
    public class ProductPictureMap : EntityMapBuilder<ProductPicture>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<ProductPicture>()
                .HasTableName(nameof(ProductPicture))
                .HasSchemaName(TableSchemaConst.Dbo)
                .HasPrimaryKey(x => x.Id)
                .HasIdentity(x => x.Id)
                .Association(x => x.Picture,
                    (productPicture, picture) => productPicture.PictureId == picture.Id)
                .Association(x => x.Product,
                    (productPicture, product) => productPicture.ProductId == product.Id);
        }
    }
}
