using Microsoft.EntityFrameworkCore;
using Camino.Core.Domain.Products;
using Camino.Shared.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camino.Infrastructure.EntityFrameworkCore.Mapping.Products
{
    public class ProductPictureMap : IEntityTypeConfiguration<ProductPicture>
    {
        public void Configure(EntityTypeBuilder<ProductPicture> builder)
        {
            builder.ToTable(nameof(ProductPicture), TableSchemaConst.Dbo)
                   .HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.HasOne(x => x.Product)
                .WithMany(x => x.ProductPictureRelations)
                .HasForeignKey(x => x.ProductId);
        }
    }
}
