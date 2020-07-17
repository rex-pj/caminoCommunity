using Camino.Core.Constants;
using Camino.Data.MapBuilders;
using Camino.Data.Entities.Identity;
using LinqToDB.Mapping;

namespace Camino.IdentityDAL.Mapping
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
