using Coco.Common.Const;
using Coco.Entities.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coco.IdentityDAL.Mapping
{
    public class RoleMap : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable(nameof(Role), TableSchemaConst.DBO);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder
               .HasMany(c => c.UserRoles)
               .WithOne(x => x.Role)
               .HasForeignKey(c => c.RoleId);

            builder
               .HasOne(c => c.CreatedBy)
               .WithMany(x => x.CreatedRoles)
               .HasForeignKey(c => c.CreatedById);

            builder
               .HasOne(c => c.UpdatedBy)
               .WithMany(x => x.UpdatedRoles)
               .HasForeignKey(c => c.UpdatedById)
               .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
