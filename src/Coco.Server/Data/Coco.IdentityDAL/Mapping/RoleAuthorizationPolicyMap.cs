using Coco.Core.Constants;
using Coco.Data.MapBuilders;
using Coco.Data.Entities.Identity;
using LinqToDB.Mapping;

namespace Coco.IdentityDAL.Mapping
{
    public class RoleAuthorizationPolicyMap : EntityMapBuilder<RoleAuthorizationPolicy>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<RoleAuthorizationPolicy>().HasTableName(nameof(RoleAuthorizationPolicy))
                .HasSchemaName(TableSchemaConst.DBO)
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
