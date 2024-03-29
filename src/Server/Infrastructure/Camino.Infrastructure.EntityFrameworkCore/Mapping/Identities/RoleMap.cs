﻿using Camino.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Camino.Core.Domains.Authorization;

namespace Camino.Infrastructure.EntityFrameworkCore.Mapping.Identities
{
    public class RoleMap : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable(nameof(Role), TableSchemas.Auth);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.ConcurrencyStamp).IsRequired(false);

            builder
               .HasMany(c => c.UserRoles)
               .WithOne(x => x.Role)
               .HasForeignKey(c => c.RoleId);

            builder
               .HasOne(c => c.CreatedBy)
               .WithMany(x => x.CreatedRoles)
               .HasForeignKey(c => c.CreatedById);

            builder
               .HasOne(c => c.UpdatedBy)
               .WithMany(x => x.UpdatedRoles)
               .HasForeignKey(c => c.UpdatedById)
               .OnDelete(DeleteBehavior.NoAction);

            builder
               .HasMany(c => c.RoleAuthorizationPolicies)
               .WithOne(x => x.Role)
               .HasForeignKey(c => c.RoleId)
               .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
