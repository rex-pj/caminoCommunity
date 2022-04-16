using Camino.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Camino.Core.Domain.Identifiers;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camino.Infrastructure.EntityFrameworkCore.Mapping.Identities
{
    public class RoleAuthorizationPolicyMap : IEntityTypeConfiguration<RoleAuthorizationPolicy>
    {
        public void Configure(EntityTypeBuilder<RoleAuthorizationPolicy> builder)
        {
            builder.ToTable(nameof(RoleAuthorizationPolicy), TableSchemaConst.Auth);
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
