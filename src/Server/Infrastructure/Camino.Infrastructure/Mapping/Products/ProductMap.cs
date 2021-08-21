using Camino.Core.Domain.Products;
using Camino.Infrastructure.Commons.Constants;
using Camino.Infrastructure.MapBuilders;
using LinqToDB.Mapping;

namespace Camino.Infrastructure.Mapping.Products
{
    public class ProductMap : EntityMapBuilder<Product>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<Product>()
                .HasTableName(nameof(Product))
                .HasSchemaName(TableSchemaConst.Dbo)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id)
                .Association(x => x.ProductCategories,
                    (product, productCategory) => product.Id == productCategory.ProductId)
                .Association(x => x.ProductPictures, 
                    (product, articlePicture) => product.Id == articlePicture.ProductId)
                .Association(x => x.ProductFarms,
                    (product, farmProduct) => product.Id == farmProduct.ProductId);
        }
    }
}
