using Coco.Common.Const;
using Coco.Entities.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coco.IdentityDAL.Mapping
{
    public class AuthorizationPolicyMap : IEntityTypeConfiguration<AuthorizationPolicy>
    {
        public void Configure(EntityTypeBuilder<AuthorizationPolicy> builder)
        {
            builder.ToTable(nameof(AuthorizationPolicy), TableSchemaConst.DBO);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder
               .HasMany(c => c.AuthorizationPolicyUsers)
               .WithOne(x => x.AuthorizationPolicy)
               .HasForeignKey(c => c.AuthorizationPolicyId);

            builder
               .HasMany(c => c.AuthorizationPolicyRoles)
               .WithOne(x => x.AuthorizationPolicy)
               .HasForeignKey(c => c.AuthorizationPolicyId);

            builder
                .HasOne(c => c.CreatedBy)
                .WithMany(x => x.CreatedAuthorizationPolicies)
                .HasForeignKey(c => c.CreatedById)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(c => c.UpdatedBy)
                .WithMany(x => x.UpdatedAuthorizationPolicies)
                .HasForeignKey(c => c.UpdatedById)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
