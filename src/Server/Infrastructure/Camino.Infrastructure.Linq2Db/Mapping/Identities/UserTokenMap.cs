using Camino.Infrastructure.Linq2Db.MapBuilders;
using Camino.Core.Domain.Identifiers;
using LinqToDB.Mapping;
using Camino.Shared.Constants;

namespace Camino.Infrastructure.Linq2Db.Mapping.Identities
{
    public class UserTokenMap : EntityMapBuilder<UserToken>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<UserToken>().HasTableName(nameof(UserToken))
                .HasSchemaName(TableSchemaConst.Auth)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id)
                .Association(c => c.User, (userToken, user) => userToken.UserId == user.Id);

            builder.Entity<UserToken>()
                .Property(x => x.Name)
                .IsNullable(false)
                .HasLength(255);

            builder.Entity<UserToken>()
                .Property(x => x.Value)
                .IsNullable(false)
                .HasLength(255);
        }
    }
}
