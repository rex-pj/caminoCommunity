using Camino.Core.Constants;
using Camino.Data.MapBuilders;
using Camino.Data.Entities.Identity;
using LinqToDB.Mapping;

namespace Camino.IdentityDAL.Mapping
{
    public class UserAttributeMap : EntityMapBuilder<UserAttribute>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<UserAttribute>().HasTableName(nameof(UserAttribute))
                .HasSchemaName(TableSchemaConst.DBO)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id);
        }
    }
}
