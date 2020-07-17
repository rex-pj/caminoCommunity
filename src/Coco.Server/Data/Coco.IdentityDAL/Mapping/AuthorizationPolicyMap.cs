using Coco.Core.Constants;
using Coco.Data.MapBuilders;
using Coco.Data.Entities.Identity;
using LinqToDB.Mapping;

namespace Coco.IdentityDAL.Mapping
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
