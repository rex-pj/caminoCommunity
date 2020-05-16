using Coco.Entities.Constant;
using Coco.Entities.Domain.Auth;
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
                .HasMany(c => c.RoleAuthorizationPolicies)
               .WithOne(x => x.Role)
               .HasForeignKey(c => c.RoleId);
        }
    }
}
