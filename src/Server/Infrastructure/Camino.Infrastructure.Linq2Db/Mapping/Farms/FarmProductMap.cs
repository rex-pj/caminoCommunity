using Camino.Infrastructure.Linq2Db.MapBuilders;
using LinqToDB.Mapping;
using Camino.Core.Domain.Farms;
using Camino.Shared.Constants;

namespace Camino.Infrastructure.Linq2Db.Mapping.Farms
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
