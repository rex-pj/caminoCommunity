using Coco.Common.Const;
using Coco.Contract.MapBuilder;
using Coco.Entities.Domain.Identity;
using LinqToDB.Mapping;

namespace Coco.IdentityDAL.Mapping
{
    public class UserLoginMap : EntityTypeBuilder<UserLogin>
    {
        public UserLoginMap(FluentMappingBuilder fluentMappingBuilder) : base(fluentMappingBuilder)
        {
        }

        public override void Configure(FluentMappingBuilder builder)
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
