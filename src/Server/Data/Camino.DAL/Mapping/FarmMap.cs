using Camino.Core.Constants;
using Camino.DAL.Entities;
using Camino.Data.MapBuilders;
using LinqToDB.Mapping;

namespace Camino.DAL.Mapping
{
    public class FarmMap : EntityMapBuilder<Farm>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<Farm>()
                .HasTableName(nameof(Farm))
                .HasSchemaName(TableSchemaConst.DBO)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id)
                .Association(x => x.FarmType,
                    (farm, farmType) => farm.FarmTypeId == farmType.Id);
        }
    }
}
