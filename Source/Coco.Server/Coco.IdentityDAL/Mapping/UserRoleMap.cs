using Coco.Common.Const;
using Coco.Contract.MapBuilder;
using Coco.Entities.Domain.Identity;
using LinqToDB.Mapping;

namespace Coco.IdentityDAL.Mapping
{
    public class UserRoleMap : EntityTypeBuilder<UserRole>
    {
        public UserRoleMap(FluentMappingBuilder fluentMappingBuilder) : base(fluentMappingBuilder)
        {
        }

        public override void Configure(FluentMappingBuilder builder)
        {
            builder.Entity<UserRole>().HasTableName(nameof(UserRole))
                .HasSchemaName(TableSchemaConst.DBO)
                .HasPrimaryKey(x => new {
                    x.UserId,
                    x.RoleId
                })
                .Association(c => c.User, (userRole, user) => userRole.UserId == user.Id)
                .Association(c => c.GrantedBy, (userRole, grantedBy) => userRole.GrantedById == grantedBy.Id);
        }
    }
}
