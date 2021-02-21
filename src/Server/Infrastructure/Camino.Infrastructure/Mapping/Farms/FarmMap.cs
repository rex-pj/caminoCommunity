using Camino.Core.Constants;
using Camino.Infrastructure.MapBuilders;
using LinqToDB.Mapping;
using Camino.Core.Domain.Farms;

namespace Camino.Infrastructure.Mapping.Farms
{
    public class FarmMap : EntityMapBuilder<Farm>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<Farm>()
                .HasTableName(nameof(Farm))
                .HasSchemaName(TableSchemaConst.Dbo)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id)
                .Association(x => x.FarmType,
                    (farm, farmType) => farm.FarmTypeId == farmType.Id);
        }
    }
}
