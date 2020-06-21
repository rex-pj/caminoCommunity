using Coco.Common.Const;
using Coco.Contract.MapBuilder;
using Coco.Entities.Domain.Identity;
using LinqToDB.Mapping;

namespace Coco.IdentityDAL.Mapping
{
    public class UserTokenMap : EntityTypeBuilder<UserToken>
    {
        public UserTokenMap(FluentMappingBuilder fluentMappingBuilder) : base(fluentMappingBuilder)
        {
        }

        public override void Configure(FluentMappingBuilder builder)
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
