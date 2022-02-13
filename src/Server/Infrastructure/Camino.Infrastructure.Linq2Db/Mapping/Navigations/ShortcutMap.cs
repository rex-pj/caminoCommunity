using Camino.Core.Domain.Navigations;
using Camino.Shared.Constants;
using Camino.Infrastructure.Linq2Db.MapBuilders;
using LinqToDB.Mapping;

namespace Camino.Infrastructure.Linq2Db.Mapping.Navigations
{
    public class ShortcutMap : EntityMapBuilder<Shortcut>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<Shortcut>()
                .HasTableName(nameof(Shortcut))
                .HasSchemaName(TableSchemaConst.Dbo)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id);
        }
    }
}
