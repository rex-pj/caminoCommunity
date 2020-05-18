using Coco.Common.Const;
using Coco.Entities.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coco.IdentityDAL.Mapping
{
    public class RoleAuthorizationPolicyMap : IEntityTypeConfiguration<RoleAuthorizationPolicy>
    {
        public void Configure(EntityTypeBuilder<RoleAuthorizationPolicy> builder)
        {
            builder.ToTable(nameof(RoleAuthorizationPolicy), TableSchemaConst.DBO);
            builder.HasKey(x => new
            {
                x.RoleId,
                x.AuthorizationPolicyId
            });

            builder
                .HasOne(c => c.GrantedBy)
                .WithMany(x => x.GrantedRoleAuthorizationPolicies)
                .HasForeignKey(c => c.GrantedById)
                .OnDelete(DeleteBehavior.NoAction);

            builder
              .HasOne(c => c.Role)
              .WithMany(x => x.RoleAuthorizationPolicies)
              .HasForeignKey(c => c.RoleId)
              .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
