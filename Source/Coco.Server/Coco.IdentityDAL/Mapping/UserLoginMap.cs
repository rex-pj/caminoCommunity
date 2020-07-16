using Coco.Core.Constants;
using Coco.Core.Infrastructure.MapBuilders;
using Coco.Core.Entities.Identity;
using LinqToDB.Mapping;

namespace Coco.IdentityDAL.Mapping
{
    public class UserLoginMap : EntityMapBuilder<UserLogin>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<UserLogin>().HasTableName(nameof(UserLogin))
                .HasSchemaName(TableSchemaConst.DBO)
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
