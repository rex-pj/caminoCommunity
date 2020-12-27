using Camino.Core.Constants;
using LinqToDB.Mapping;
using Camino.Data.MapBuilders;
using Camino.DAL.Entities;

namespace Camino.DAL.Mapping
{
    public class ProductPictureMap : EntityMapBuilder<ProductPicture>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<ProductPicture>()
                .HasTableName(nameof(ProductPicture))
                .HasSchemaName(TableSchemaConst.DBO)
                .HasPrimaryKey(x => x.Id)
                .HasIdentity(x => x.Id)
                .Association(x => x.Picture,
                    (productPicture, picture) => productPicture.PictureId == picture.Id)
                .Association(x => x.Product,
                    (productPicture, product) => productPicture.ProductId == product.Id);
        }
    }
}
