using Camino.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Camino.Core.Domains.Authorization;

namespace Camino.Infrastructure.EntityFrameworkCore.Mapping.Identities
{
    public class RoleAuthorizationPolicyMap : IEntityTypeConfiguration<RoleAuthorizationPolicy>
    {
        public void Configure(EntityTypeBuilder<RoleAuthorizationPolicy> builder)
        {
            builder.ToTable(nameof(RoleAuthorizationPolicy), TableSchemas.Auth);
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
