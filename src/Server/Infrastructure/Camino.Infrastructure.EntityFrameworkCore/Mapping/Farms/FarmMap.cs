﻿using Camino.Shared.Constants;
using Camino.Core.Domains.Farms;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Camino.Infrastructure.EntityFrameworkCore.Mapping.Farms
{
    public class FarmMap : IEntityTypeConfiguration<Farm>
    {
        public void Configure(EntityTypeBuilder<Farm> builder)
        {
            builder.ToTable(nameof(Farm), TableSchemas.Dbo)
                .HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            
            builder.HasOne(x => x.FarmType)
                .WithMany(x => x.Farms)
                .HasForeignKey(x => x.FarmTypeId);
        }
    }
}