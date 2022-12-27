using Microsoft.EntityFrameworkCore;
using Camino.Core.Domains.Media;
using Camino.Shared.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camino.Infrastructure.EntityFrameworkCore.Mapping.Media
{
    public class PictureMap : IEntityTypeConfiguration<Picture>
    {
        public void Configure(EntityTypeBuilder<Picture> builder)
        {
            builder.ToTable(nameof(Picture), TableSchemas.Dbo);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Title).IsRequired(false);
            builder.Property(x => x.Alt).IsRequired(false);
            builder.Property(x => x.RelativePath).IsRequired(false);
            builder.Property(x => x.BinaryData).IsRequired(false);
        }
    }
}
