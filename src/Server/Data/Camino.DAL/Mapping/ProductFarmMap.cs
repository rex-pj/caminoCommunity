using Camino.Core.Constants;
using Camino.DAL.Entities;
using Camino.Data.MapBuilders;
using LinqToDB.Mapping;

namespace Camino.DAL.Mapping
{
    public class ProductFarmMap : EntityMapBuilder<FarmProduct>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<FarmProduct>()
                .HasTableName(nameof(FarmProduct))
                .HasSchemaName(TableSchemaConst.DBO)
                .HasPrimaryKey(x => x.Id)
                .HasIdentity(x => x.Id)
                .Association(x => x.Product,
                    (productFarm, product) => productFarm.ProductId == product.Id)
                .Association(x => x.Farm,
                    (productFarm, farm) => productFarm.FarmId == farm.Id);
        }
    }
}
