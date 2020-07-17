using Camino.Core.Constants;
using Camino.Data.MapBuilders;
using Camino.Data.Entities.Identity;
using LinqToDB.Mapping;

namespace Camino.IdentityDAL.Mapping
{
    public class RoleClaimMap : EntityMapBuilder<RoleClaim>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<RoleClaim>().HasTableName(nameof(RoleClaim))
                .HasSchemaName(TableSchemaConst.DBO)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id)
                .Association(c => c.Role, (roleClaim, role) => roleClaim.RoleId == role.Id);
        }
    }
}
