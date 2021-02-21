using Camino.Core.Constants;
using Camino.Infrastructure.MapBuilders;
using Camino.Core.Domain.Media;
using LinqToDB.Mapping;

namespace Camino.Infrastructure.Mapping.Media
{
    public class PictureMap : EntityMapBuilder<Picture>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<Picture>()
                .HasTableName(nameof(Picture))
                .HasSchemaName(TableSchemaConst.Dbo)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id);
        }
    }
}
