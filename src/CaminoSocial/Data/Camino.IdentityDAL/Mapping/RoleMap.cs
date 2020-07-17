using Camino.Core.Constants;
using Camino.Data.MapBuilders;
using Camino.Data.Entities.Identity;
using LinqToDB.Mapping;

namespace Camino.IdentityDAL.Mapping
{
    public class RoleMap : EntityMapBuilder<Role>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<Role>().HasTableName(nameof(Role))
                .HasSchemaName(TableSchemaConst.DBO)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id)
                .Association(c => c.UserRoles, (role, userRoles) => role.Id == userRoles.RoleId)
                .Association(c => c.CreatedBy, (role, createdBy) => role.CreatedById == createdBy.Id)
                .Association(c => c.UpdatedBy, (role, updatedBy) => role.UpdatedById == updatedBy.Id)
                .Association(c => c.RoleAuthorizationPolicies, (role, roleAuthorizationPolicy) => role.Id == roleAuthorizationPolicy.RoleId);
        }
    }
}
