﻿using Camino.Core.Domains.Products;
using Camino.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camino.Infrastructure.EntityFrameworkCore.Mapping.Products
{
    public class ProductPriceMap : IEntityTypeConfiguration<ProductPrice>
    {
        public void Configure(EntityTypeBuilder<ProductPrice> builder)
        {
            builder.ToTable(nameof(ProductPrice), TableSchemas.Dbo)
                   .HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Price).HasPrecision(18, 2);

            builder.HasOne(x => x.Product)
                .WithMany(x => x.ProductPriceRelations)
                .HasForeignKey(x => x.ProductId);
        }
    }
}
