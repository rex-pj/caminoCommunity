using Camino.Core.Constants;
using Camino.Data.MapBuilders;
using Camino.IdentityDAL.Entities;
using LinqToDB.Mapping;

namespace Camino.IdentityDAL.Mapping
{
    public class UserMap : EntityMapBuilder<User>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<User>()
                .HasTableName(nameof(User))
                .HasSchemaName(TableSchemaConst.DBO)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id)
                .Association(e => e.UserInfo, (user, userInfo) => user.Id == userInfo.Id)
                .Association(e => e.CreatedBy, (user, createdBy) => user.CreatedById == createdBy.Id)
                .Association(e => e.CreatedUsers, (user, createdUsers) => user.Id == createdUsers.Id)
                .Association(e => e.UpdatedBy, (user, updatedBy) => user.UpdatedById == updatedBy.Id)
                .Association(e => e.UpdatedUsers, (user, updatedUsers) => user.Id == updatedUsers.Id)
                .Association(e => e.UserRoles, (user, userRoles) => user.Id == userRoles.UserId)
                .Association(e => e.CreatedRoles, (user, role) => user.Id == role.CreatedById)
                .Association(e => e.UpdatedRoles, (user, role) => user.Id == role.UpdatedById)
                .Association(e => e.Status, (user, status) => user.StatusId == status.Id)
                .Association(e => e.CreatedAuthorizationPolicies, 
                    (user, authorizationPolicy) => user.Id == authorizationPolicy.CreatedById)
                .Association(e => e.UpdatedAuthorizationPolicies,
                    (user, authorizationPolicy) => user.Id == authorizationPolicy.UpdatedById)
                .Association(e => e.UserAuthorizationPolicies,
                    (user, userAuthorizationPolicy) => user.Id == userAuthorizationPolicy.UserId)
                .Association(e => e.GrantedRoleAuthorizationPolicies,
                    (user, roleAuthorizationPolicy) => user.Id == roleAuthorizationPolicy.GrantedById);
        }
    }
}
