using Microsoft.EntityFrameworkCore;
using Camino.Shared.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Camino.Core.Domains.Authorization;

namespace Camino.Infrastructure.EntityFrameworkCore.Mapping.Identities
{
    public class UserClaimMap : IEntityTypeConfiguration<UserClaim>
    {
        public void Configure(EntityTypeBuilder<UserClaim> builder)
        {
            builder.ToTable(nameof(UserClaim), TableSchemas.Auth);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder
               .HasOne(c => c.User)
               .WithMany(x => x.UserClaims)
               .HasForeignKey(c => c.UserId);
        }
    }
}
