using Camino.Shared.Constants;
using Camino.Core.Domain.Products;
using Camino.Infrastructure.Linq2Db.MapBuilders;
using LinqToDB.Mapping;

namespace Camino.Infrastructure.Linq2Db.Mapping.Products
{
    public class ProductAttributeRelationValueMap : EntityMapBuilder<ProductAttributeRelationValue>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<ProductAttributeRelationValue>()
                .HasTableName(nameof(ProductAttributeRelationValue))
                .HasSchemaName(TableSchemaConst.Dbo)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id)
                .Association(x => x.ProductAttributeRelation,
                    (relationValue, relation) => relationValue.ProductAttributeRelationId == relation.Id);
        }
    }
}