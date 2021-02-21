using Camino.Core.Constants;
using Camino.Core.Domain.Farms;
using Camino.Infrastructure.MapBuilders;
using LinqToDB.Mapping;

namespace Camino.Infrastructure.Mapping.Farms
{
    public class FarmTypeMap : EntityMapBuilder<FarmType>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<FarmType>()
                .HasTableName(nameof(FarmType))
                .HasSchemaName(TableSchemaConst.Dbo)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id);
        }
    }
}
