﻿using Microsoft.EntityFrameworkCore;
using Camino.Shared.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Camino.Core.Domains.Authentication;

namespace Camino.Infrastructure.EntityFrameworkCore.Mapping.Identities
{
    public class UserTokenMap : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            builder.ToTable(nameof(UserToken), TableSchemas.Auth);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.Value)
                .IsRequired()
                .HasMaxLength(255);

            builder
               .HasOne(c => c.User)
               .WithMany(x => x.UserTokens)
               .HasForeignKey(c => c.UserId);
        }
    }
}
