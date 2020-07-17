using Coco.Core.Constants;
using Coco.Data.MapBuilders;
using Coco.Data.Entities.Identity;
using LinqToDB.Mapping;

namespace Coco.IdentityDAL.Mapping
{
    public class RoleClaimMap : EntityMapBuilder<RoleClaim>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<RoleClaim>().HasTableName(nameof(RoleClaim))
                .HasSchemaName(TableSchemaConst.DBO)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id)
                .Association(c => c.Role, (roleClaim, role) => roleClaim.RoleId == role.Id);
        }
    }
}
