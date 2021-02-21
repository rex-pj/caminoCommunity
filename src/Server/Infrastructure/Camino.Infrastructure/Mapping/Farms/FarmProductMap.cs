using Camino.Core.Constants;
using Camino.Infrastructure.MapBuilders;
using LinqToDB.Mapping;
using Camino.Core.Domain.Farms;

namespace Camino.Infrastructure.Mapping.Farms
{
    public class FarmProductMap : EntityMapBuilder<FarmProduct>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<FarmProduct>()
                .HasTableName(nameof(FarmProduct))
                .HasSchemaName(TableSchemaConst.Dbo)
                .HasPrimaryKey(x => x.Id)
                .HasIdentity(x => x.Id)
                .Association(x => x.Product,
                    (productFarm, product) => productFarm.ProductId == product.Id)
                .Association(x => x.Farm,
                    (productFarm, farm) => productFarm.FarmId == farm.Id);
        }
    }
}
