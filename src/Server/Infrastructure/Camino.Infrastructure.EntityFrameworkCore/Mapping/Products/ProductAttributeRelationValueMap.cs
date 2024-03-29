﻿using Camino.Shared.Constants;
using Camino.Core.Domains.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camino.Infrastructure.EntityFrameworkCore.Mapping.Products
{
    public class ProductAttributeRelationValueMap : IEntityTypeConfiguration<ProductAttributeRelationValue>
    {
        public void Configure(EntityTypeBuilder<ProductAttributeRelationValue> builder)
        {
            builder
                .ToTable(nameof(ProductAttributeRelationValue), TableSchemas.Dbo)
                .HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.PriceAdjustment).HasPrecision(18, 2);
            builder.Property(x => x.PricePercentageAdjustment).HasPrecision(18, 2);

            builder.HasOne(x => x.ProductAttributeRelation)
                .WithMany(x => x.ProductAttributeRelationValues)
                .HasForeignKey(x => x.ProductAttributeRelationId);
        }
    }
}