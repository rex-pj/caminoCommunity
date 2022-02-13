using Camino.Shared.Constants;
using Camino.Core.Domain.Farms;
using Camino.Infrastructure.Linq2Db.MapBuilders;
using LinqToDB.Mapping;

namespace Camino.Infrastructure.Linq2Db.Mapping.Farms
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
