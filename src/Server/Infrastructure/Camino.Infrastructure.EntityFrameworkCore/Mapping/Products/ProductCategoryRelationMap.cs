using Camino.Core.Domain.Products;
using Camino.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camino.Infrastructure.EntityFrameworkCore.Mapping.Products
{
    public class ProductCategoryRelationMap : IEntityTypeConfiguration<ProductCategoryRelation>
    {
        public void Configure(EntityTypeBuilder<ProductCategoryRelation> builder)
        {
            builder
                   .ToTable(nameof(ProductCategoryRelation), TableSchemaConst.Dbo)
                   .HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.HasOne(x => x.Product)
                .WithMany(x => x.ProductCategoryRelations)
                .HasForeignKey(x => x.ProductCategoryId);

            builder.HasOne(x => x.ProductCategory)
                .WithMany(x => x.ProductCategoryRelations)
                .HasForeignKey(x => x.ProductCategoryId);
        }
    }
}
