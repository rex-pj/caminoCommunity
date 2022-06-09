using Camino.Core.Domains.Farms;
using Camino.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camino.Infrastructure.EntityFrameworkCore.Mapping.Farms
{
    public class FarmPictureMap : IEntityTypeConfiguration<FarmPicture>
    {
        public void Configure(EntityTypeBuilder<FarmPicture> builder)
        {
            builder.ToTable(nameof(FarmPicture), TableSchemas.Dbo)
                .HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}
