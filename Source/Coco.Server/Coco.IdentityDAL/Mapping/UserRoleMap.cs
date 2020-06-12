using Coco.Common.Const;
using Coco.Entities.Domain.Identity;
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

            builder
                .HasOne(c => c.User)
                .WithMany(x => x.UserRoles)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(c => c.GrantedBy)
                .WithMany(x => x.GrantedUserRoles)
                .HasForeignKey(c => c.GrantedById)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}