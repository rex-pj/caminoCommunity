using Camino.Core.Constants;
using Camino.Data.MapBuilders;
using Camino.IdentityDAL.Entities;
using LinqToDB.Mapping;

namespace Camino.IdentityDAL.Mapping
{
    public class UserTokenMap : EntityMapBuilder<UserToken>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<UserToken>().HasTableName(nameof(UserToken))
                .HasSchemaName(TableSchemaConst.DBO)
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
