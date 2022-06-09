using Camino.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Camino.Core.Domains.Users;

namespace Camino.Infrastructure.EntityFrameworkCore.Mapping.Identities
{
    public class GenderMap : IEntityTypeConfiguration<Gender>
    {
        public void Configure(EntityTypeBuilder<Gender> builder)
        {
            builder
               .ToTable(nameof(Gender), TableSchemas.Auth)
               .HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.HasMany(x => x.UserInfos)
                .WithOne(x => x.Gender)
                .HasForeignKey(x => x.GenderId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
