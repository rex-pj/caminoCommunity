using Camino.Shared.Constants;
using Camino.Core.Domains.Articles;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Camino.Infrastructure.EntityFrameworkCore.Mapping.Articles
{
    public class ArticleMap : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.ToTable(nameof(Article), TableSchemas.Dbo)
                .HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).HasMaxLength(255);

            builder.Property(x => x.Description).HasMaxLength(1000).IsRequired(false);
            builder.Property(x => x.Content).HasMaxLength(8000);
            builder.HasOne(x => x.ArticleCategory)
                .WithMany(x => x.Articles)
                .HasForeignKey(x => x.ArticleCategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
