using Coco.Common.Const;
using Coco.Entities.Domain.Content;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coco.DAL.Mapping
{
    public class ArticleCategoryMap : IEntityTypeConfiguration<ArticleCategory>
    {
        public void Configure(EntityTypeBuilder<ArticleCategory> builder)
        {
            builder.ToTable(nameof(ArticleCategory), TableSchemaConst.DBO);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.HasOne(x => x.ParentCategory)
                .WithMany(x => x.ChildCategories)
                .HasForeignKey(x => x.ParentCategoryId);
        }
    }
}
