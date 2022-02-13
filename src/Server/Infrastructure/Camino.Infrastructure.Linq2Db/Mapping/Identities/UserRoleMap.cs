using Camino.Shared.Constants;
using Camino.Infrastructure.Linq2Db.MapBuilders;
using Camino.Core.Domain.Identifiers;
using LinqToDB.Mapping;

namespace Camino.Infrastructure.Linq2Db.Mapping.Identities
{
    public class UserRoleMap : EntityMapBuilder<UserRole>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<UserRole>().HasTableName(nameof(UserRole))
                .HasSchemaName(TableSchemaConst.Auth)
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
