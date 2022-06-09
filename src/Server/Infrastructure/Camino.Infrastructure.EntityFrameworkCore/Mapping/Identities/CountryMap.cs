using Camino.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Camino.Core.Domains.Identifiers;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camino.Infrastructure.EntityFrameworkCore.Mapping.Identities
{
    public class CountryMap : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder
                .ToTable(nameof(Country), TableSchemas.Auth)
                .HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.HasMany(x => x.UserInfos)
                .WithOne(x => x.Country)
                .HasForeignKey(x => x.CountryId);
        }
    }
}
