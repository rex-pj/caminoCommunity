using Coco.Entities.Constant;
using Coco.Entities.Domain.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coco.IdentityDAL.Mapping
{
    public class UserRoleMap : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable(nameof(UserRole), TableSchemaConst.DBO);
            builder.HasKey(x => new
            {
                x.UserId,
                x.RoleId
            });
        }
    }
}