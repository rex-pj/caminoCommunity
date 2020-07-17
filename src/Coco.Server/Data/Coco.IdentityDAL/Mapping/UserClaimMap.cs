using Coco.Core.Constants;
using Coco.Data.MapBuilders;
using Coco.Data.Entities.Identity;
using LinqToDB.Mapping;

namespace Coco.IdentityDAL.Mapping
{
    public class UserClaimMap : EntityMapBuilder<UserClaim>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<UserClaim>().HasTableName(nameof(UserClaim))
                .HasSchemaName(TableSchemaConst.DBO)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id)
                .Association(c => c.User, (userClaim, user) => userClaim.UserId == user.Id);
        }
    }
}
