using Camino.Infrastructure.MapBuilders;
using Camino.Core.Domain.Identifiers;
using LinqToDB.Mapping;
using Camino.Infrastructure.Commons.Constants;

namespace Camino.Infrastructure.Mapping.Identities
{
    public class RoleClaimMap : EntityMapBuilder<RoleClaim>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<RoleClaim>().HasTableName(nameof(RoleClaim))
                .HasSchemaName(TableSchemaConst.Auth)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id)
                .Association(c => c.Role, (roleClaim, role) => roleClaim.RoleId == role.Id);
        }
    }
}
