using Camino.Shared.Constants;
using Camino.Core.Domain.Farms;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Camino.Infrastructure.EntityFrameworkCore.Mapping.Farms
{
    public class FarmTypeMap : IEntityTypeConfiguration<FarmType>
    {
        public void Configure(EntityTypeBuilder<FarmType> builder)
        {
            builder
                .ToTable(nameof(FarmType), TableSchemaConst.Dbo)
                .HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.HasMany(x => x.Farms)
                .WithOne(x => x.FarmType)
                .HasForeignKey(x => x.FarmTypeId);
        }
    }
}
