using Camino.Core.Constants;
using Camino.Core.Domain.Products;
using Camino.Infrastructure.MapBuilders;
using LinqToDB.Mapping;

namespace Camino.Infrastructure.Mapping.Products
{
    public class ProductAttributeRelationMap : EntityMapBuilder<ProductAttributeRelation>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<ProductAttributeRelation>()
                .HasTableName(nameof(ProductAttributeRelation))
                .HasSchemaName(TableSchemaConst.Dbo)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id)
                .Association(x => x.ProductAttribute,
                    (relation, attribute) => relation.ProductAttributeId == attribute.Id)
                .Association(x => x.ProductAttributeRelationValues,
                    (relation, relationValue) => relation.Id == relationValue.ProductAttributeRelationId);
        }
    }
}
