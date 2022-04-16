using Camino.Core.Domain.Products;
using Camino.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camino.Infrastructure.EntityFrameworkCore.Mapping.Products
{
    public class ProductAttributeRelationMap : IEntityTypeConfiguration<ProductAttributeRelation>
    {
        public void Configure(EntityTypeBuilder<ProductAttributeRelation> builder)
        {
            builder
                .ToTable(nameof(ProductAttributeRelation), TableSchemaConst.Dbo)
                .HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.HasMany(x => x.ProductAttributeRelationValues)
                .WithOne(x => x.ProductAttributeRelation)
                .HasForeignKey(x => x.ProductAttributeRelationId);

            builder.HasOne(x => x.ProductAttribute)
                .WithMany(x => x.ProductAttributeRelations)
                .HasForeignKey(x => x.ProductAttributeId);
        }
    }
}
