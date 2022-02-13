using Camino.Infrastructure.Linq2Db.MapBuilders;
using Camino.Core.Domain.Identifiers;
using LinqToDB.Mapping;
using Camino.Shared.Constants;

namespace Camino.Infrastructure.Linq2Db.Mapping.Identities
{
    public class UserLoginMap : EntityMapBuilder<UserLogin>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<UserLogin>().HasTableName(nameof(UserLogin))
                .HasSchemaName(TableSchemaConst.Auth)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id)
                .Association(c => c.User, (userLogin, user) => userLogin.UserId == user.Id);

            builder.Entity<UserLogin>()
                .Property(x => x.ProviderDisplayName)
                .IsNullable(false)
                .HasLength(255);

            builder.Entity<UserLogin>()
                .Property(x => x.LoginProvider)
                .IsNullable(false)
                .HasLength(255);

            builder.Entity<UserLogin>()
                .Property(x => x.ProviderKey)
                .IsNullable(false);
        }
    }
}
