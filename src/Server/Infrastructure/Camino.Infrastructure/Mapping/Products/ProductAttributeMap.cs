using Camino.Infrastructure.Commons.Constants;
using Camino.Core.Domain.Products;
using Camino.Infrastructure.MapBuilders;
using LinqToDB.Mapping;

namespace Camino.Infrastructure.Mapping.Products
{
    public class ProductAttributeMap : EntityMapBuilder<ProductAttribute>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<ProductAttribute>()
                .HasTableName(nameof(ProductAttribute))
                .HasSchemaName(TableSchemaConst.Dbo)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id)
                .Association(x => x.ProductAttributeRelations,
                    (attribute, relation) => attribute.Id == relation.ProductAttributeId);
        }
    }
}
