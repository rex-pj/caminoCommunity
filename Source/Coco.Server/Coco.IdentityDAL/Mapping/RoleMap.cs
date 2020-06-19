using Coco.Common.Const;
using Coco.Contract.MapBuilder;
using Coco.Entities.Domain.Identity;
using LinqToDB.Mapping;

namespace Coco.IdentityDAL.Mapping
{
    public class RoleMap : EntityTypeBuilder<Role>
    {
        public RoleMap(FluentMappingBuilder fluentMappingBuilder) : base(fluentMappingBuilder)
        {
        }

        public override void Configure(FluentMappingBuilder builder)
        {
            builder.Entity<Role>().HasTableName(nameof(Role))
                .HasSchemaName(TableSchemaConst.DBO)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id)
                .Association(c => c.UserRoles, (role, userRoles) => role.Id == userRoles.RoleId)
                .Association(c => c.CreatedBy, (role, createdBy) => role.CreatedById == createdBy.Id)
                .Association(c => c.UpdatedBy, (role, updatedBy) => role.UpdatedById == updatedBy.Id);
        }
    }
}
