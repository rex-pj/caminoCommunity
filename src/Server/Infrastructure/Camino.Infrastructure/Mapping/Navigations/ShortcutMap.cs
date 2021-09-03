using Camino.Core.Domain.Navigations;
using Camino.Infrastructure.Commons.Constants;
using Camino.Infrastructure.MapBuilders;
using LinqToDB.Mapping;

namespace Camino.Infrastructure.Mapping.Navigations
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
