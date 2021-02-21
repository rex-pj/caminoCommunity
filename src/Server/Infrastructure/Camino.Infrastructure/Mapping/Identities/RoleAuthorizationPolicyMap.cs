using Camino.Core.Constants;
using Camino.Infrastructure.MapBuilders;
using Camino.Core.Domain.Identifiers;
using LinqToDB.Mapping;

namespace Camino.Infrastructure.Mapping.Identities
{
    public class RoleAuthorizationPolicyMap : EntityMapBuilder<RoleAuthorizationPolicy>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<RoleAuthorizationPolicy>().HasTableName(nameof(RoleAuthorizationPolicy))
                .HasSchemaName(TableSchemaConst.Auth)
                .HasPrimaryKey(x => new
                {
                    x.RoleId,
                    x.AuthorizationPolicyId
                })
                .Association(c => c.GrantedBy, (roleAuthorizationPolicy, grantedBy) => roleAuthorizationPolicy.GrantedById == grantedBy.Id)
                .Association(c => c.AuthorizationPolicy, 
                    (roleAuthorizationPolicy, authorizationPolicy) => roleAuthorizationPolicy.AuthorizationPolicyId == authorizationPolicy.Id)
                .Association(c => c.Role, (roleAuthorizationPolicy, role) => roleAuthorizationPolicy.RoleId == role.Id);
        }
    }
}
