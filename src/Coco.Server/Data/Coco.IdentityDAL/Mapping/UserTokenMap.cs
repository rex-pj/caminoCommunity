using Coco.Core.Constants;
using Coco.Data.MapBuilders;
using Coco.Data.Entities.Identity;
using LinqToDB.Mapping;

namespace Coco.IdentityDAL.Mapping
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
