using Camino.Core.Domains.Farms;
using Camino.Shared.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Camino.Infrastructure.EntityFrameworkCore.Mapping.Farms
{
    public class FarmProductMap : IEntityTypeConfiguration<FarmProduct>
    {
        public void Configure(EntityTypeBuilder<FarmProduct> builder)
        {
            builder.ToTable(nameof(FarmProduct), TableSchemas.Dbo)
                .HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}
