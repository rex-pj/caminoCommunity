using Camino.Shared.Constants;
using Camino.Core.Domains.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camino.Infrastructure.EntityFrameworkCore.Mapping.Products
{
    public class ProductAttributeMap : IEntityTypeConfiguration<ProductAttribute>
    {
        public void Configure(EntityTypeBuilder<ProductAttribute> builder)
        {
            builder
                .ToTable(nameof(ProductAttribute), TableSchemas.Dbo)
                .HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.HasMany(x => x.ProductAttributeRelations)
                .WithOne(x => x.ProductAttribute)
                .HasForeignKey(x => x.ProductAttributeId);
        }
    }
}
