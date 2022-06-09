using Camino.Core.Domains.Products;
using Camino.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camino.Infrastructure.EntityFrameworkCore.Mapping.Products
{
    public class ProductMap : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable(nameof(Product), TableSchemas.Dbo)
                   .HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.HasMany(x => x.ProductPictureRelations)
                .WithOne(x => x.Product)
                .HasForeignKey(x => x.ProductId);

            builder.HasMany(x => x.ProductCategoryRelations)
                .WithOne(x => x.Product)
                .HasForeignKey(x => x.ProductId);

            builder.HasMany(x => x.ProductFarmRelations)
                .WithOne(x => x.Product)
                .HasForeignKey(x => x.ProductId);
        }
    }
}
