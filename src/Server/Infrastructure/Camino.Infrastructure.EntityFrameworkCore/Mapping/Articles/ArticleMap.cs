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

            builder.Property(x => x.Description).IsRequired(false);
            builder.HasOne(x => x.ArticleCategory)
                .WithMany(x => x.Articles)
                .HasForeignKey(x => x.ArticleCategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
