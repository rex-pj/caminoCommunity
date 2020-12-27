using Camino.Core.Constants;
using Camino.DAL.Entities;
using Camino.Data.MapBuilders;
using LinqToDB.Mapping;

namespace Camino.DAL.Mapping
{
    public class FarmTypeMap : EntityMapBuilder<FarmType>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<FarmType>()
                .HasTableName(nameof(FarmType))
                .HasSchemaName(TableSchemaConst.DBO)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id);
        }
    }
}
