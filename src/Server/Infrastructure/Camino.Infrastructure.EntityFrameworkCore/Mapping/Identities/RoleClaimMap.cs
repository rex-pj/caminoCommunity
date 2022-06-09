using Microsoft.EntityFrameworkCore;
using Camino.Shared.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Camino.Core.Domains.Authorization;

namespace Camino.Infrastructure.EntityFrameworkCore.Mapping.Identities
{
    public class RoleClaimMap : IEntityTypeConfiguration<RoleClaim>
    {
        public void Configure(EntityTypeBuilder<RoleClaim> builder)
        {
            builder.ToTable(nameof(RoleClaim), TableSchemas.Auth);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder
               .HasOne(c => c.Role)
               .WithMany(x => x.RoleClaims)
               .HasForeignKey(c => c.RoleId);
        }
    }
}
