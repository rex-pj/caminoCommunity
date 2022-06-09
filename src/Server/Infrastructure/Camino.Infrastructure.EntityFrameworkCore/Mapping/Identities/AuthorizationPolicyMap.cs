using Camino.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Camino.Core.Domains.Authorization;

namespace Camino.Infrastructure.EntityFrameworkCore.Mapping.Identities
{
    public class AuthorizationPolicyMap : IEntityTypeConfiguration<AuthorizationPolicy>
    {
        public void Configure(EntityTypeBuilder<AuthorizationPolicy> builder)
        {
            builder
                .ToTable(nameof(AuthorizationPolicy), TableSchemas.Auth)
                .HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.HasMany(x => x.AuthorizationPolicyUsers)
                .WithOne(x => x.AuthorizationPolicy)
                .HasForeignKey(x => x.AuthorizationPolicyId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(x => x.AuthorizationPolicyRoles)
                .WithOne(x => x.AuthorizationPolicy)
                .HasForeignKey(x => x.AuthorizationPolicyId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.CreatedBy)
                .WithMany(x => x.CreatedAuthorizationPolicies)
                .HasForeignKey(x => x.CreatedById)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.UpdatedBy)
                .WithMany(x => x.UpdatedAuthorizationPolicies)
                .HasForeignKey(x => x.UpdatedById)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
