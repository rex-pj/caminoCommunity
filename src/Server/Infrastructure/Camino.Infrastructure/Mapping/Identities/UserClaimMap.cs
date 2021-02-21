using Camino.Core.Constants;
using Camino.Infrastructure.MapBuilders;
using Camino.Core.Domain.Identifiers;
using LinqToDB.Mapping;

namespace Camino.Infrastructure.Mapping.Identities
{
    public class UserClaimMap : EntityMapBuilder<UserClaim>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<UserClaim>().HasTableName(nameof(UserClaim))
                .HasSchemaName(TableSchemaConst.Auth)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id)
                .Association(c => c.User, (userClaim, user) => userClaim.UserId == user.Id);
        }
    }
}
