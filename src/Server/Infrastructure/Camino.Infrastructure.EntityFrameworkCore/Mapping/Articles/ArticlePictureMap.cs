using Camino.Shared.Constants;
using Camino.Core.Domains.Articles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camino.Infrastructure.EntityFrameworkCore.Mapping.Articles
{
    public class ArticlePictureMap : IEntityTypeConfiguration<ArticlePicture>
    {
        public void Configure(EntityTypeBuilder<ArticlePicture> builder)
        {
            builder.ToTable(nameof(ArticlePicture), TableSchemas.Dbo)
                .HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}
