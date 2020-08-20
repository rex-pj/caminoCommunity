using Camino.Core.Constants;
using Camino.Data.MapBuilders;
using Camino.IdentityDAL.Entities;
using LinqToDB.Mapping;

namespace Camino.IdentityDAL.Mapping
{
    public class AuthorizationPolicyMap : EntityMapBuilder<AuthorizationPolicy>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<AuthorizationPolicy>()
                .HasTableName(nameof(AuthorizationPolicy))
                .HasSchemaName(TableSchemaConst.DBO)
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
