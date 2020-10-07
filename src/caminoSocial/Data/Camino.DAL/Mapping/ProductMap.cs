using Camino.Core.Constants;
using Camino.DAL.Entities;
using Camino.Data.MapBuilders;
using LinqToDB.Mapping;

namespace Camino.DAL.Mapping
{
    public class ProductMap : EntityMapBuilder<Product>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<Product>()
                .HasTableName(nameof(Product))
                .HasSchemaName(TableSchemaConst.DBO)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id)
                .Association(x => x.ProductCategories,
                    (product, productCategory) => product.Id == productCategory.ProductId)
                .Association(x => x.ProductPictures, 
                    (product, articlePicture) => product.Id == articlePicture.ProductId);
        }
    }
}
