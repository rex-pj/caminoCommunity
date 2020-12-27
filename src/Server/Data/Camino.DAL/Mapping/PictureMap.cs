using Camino.Core.Constants;
using Camino.DAL.Entities;
using Camino.Data.MapBuilders;
using LinqToDB.Mapping;

namespace Camino.DAL.Mapping
{
    public class PictureMap : EntityMapBuilder<Picture>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<Picture>()
                .HasTableName(nameof(Picture))
                .HasSchemaName(TableSchemaConst.DBO)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id);
        }
    }
}
