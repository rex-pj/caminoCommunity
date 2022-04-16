using Camino.Core.Domain.Products;
using Camino.Shared.Constants;
using Camino.Infrastructure.Linq2Db.MapBuilders;
using LinqToDB.Mapping;

namespace Camino.Infrastructure.Linq2Db.Mapping.Products
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
                .Association(x => x.ProductCategoryRelations,
                    (product, productCategory) => product.Id == productCategory.ProductId)
                .Association(x => x.ProductPictureRelations, 
                    (product, articlePicture) => product.Id == articlePicture.ProductId)
                .Association(x => x.ProductFarmRelations,
                    (product, farmProduct) => product.Id == farmProduct.ProductId);
        }
    }
}
