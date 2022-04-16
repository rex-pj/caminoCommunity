using Camino.Shared.Constants;
using Camino.Core.Domain.Articles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camino.Infrastructure.EntityFrameworkCore.Mapping.Articles
{
    public class ArticlePictureMap : IEntityTypeConfiguration<ArticlePicture>
    {
        public void Configure(EntityTypeBuilder<ArticlePicture> builder)
        {
            builder.ToTable(nameof(ArticlePicture), TableSchemaConst.Dbo)
                .HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}
