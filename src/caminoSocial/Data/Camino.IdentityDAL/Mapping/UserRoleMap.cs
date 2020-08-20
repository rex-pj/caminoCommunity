using Camino.Core.Constants;
using Camino.Data.MapBuilders;
using Camino.IdentityDAL.Entities;
using LinqToDB.Mapping;

namespace Camino.IdentityDAL.Mapping
{
    public class UserRoleMap : EntityMapBuilder<UserRole>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<UserRole>().HasTableName(nameof(UserRole))
                .HasSchemaName(TableSchemaConst.DBO)
                .HasPrimaryKey(x => new {
                    x.UserId,
                    x.RoleId
                })
                .Association(c => c.User, (userRole, user) => userRole.UserId == user.Id)
                .Association(c => c.Role, (userRole, role) => userRole.RoleId == role.Id)
                .Association(c => c.GrantedBy, (userRole, grantedBy) => userRole.GrantedById == grantedBy.Id);
        }
    }
}
