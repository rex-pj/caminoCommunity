using Camino.Core.Constants;
using Camino.Infrastructure.MapBuilders;
using Camino.Core.Domain.Identifiers;
using LinqToDB.Mapping;

namespace Camino.Infrastructure.Mapping.Identities
{
    public class AuthorizationPolicyMap : EntityMapBuilder<AuthorizationPolicy>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<AuthorizationPolicy>()
                .HasTableName(nameof(AuthorizationPolicy))
                .HasSchemaName(TableSchemaConst.Auth)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id)
                .Association(c => c.AuthorizationPolicyUsers, 
                    (authorizationPolicy, userAuthorizationPolicy) => authorizationPolicy.Id == userAuthorizationPolicy.AuthorizationPolicyId)
                .Association(c => c.AuthorizationPolicyRoles,
                    (authorizationPolicy, roleAuthorizationPolicy) => authorizationPolicy.Id == roleAuthorizationPolicy.AuthorizationPolicyId)
                .Association(c => c.CreatedBy, (authorizationPolicy, createdBy) => authorizationPolicy.CreatedById == createdBy.Id)
                .Association(c => c.UpdatedBy, (authorizationPolicy, updatedBy) => authorizationPolicy.UpdatedById == updatedBy.Id);
        }
    }
}
