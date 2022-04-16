﻿using Camino.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Camino.Core.Domain.Identifiers;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camino.Infrastructure.EntityFrameworkCore.Mapping.Identities
{
    public class UserAuthorizationPolicyMap : IEntityTypeConfiguration<UserAuthorizationPolicy>
    {
        public void Configure(EntityTypeBuilder<UserAuthorizationPolicy> builder)
        {
            builder.ToTable(nameof(UserAuthorizationPolicy), TableSchemaConst.Auth);
            builder.HasKey(x => new
            {
                x.UserId,
                x.AuthorizationPolicyId
            });

            builder
               .HasOne(c => c.User)
               .WithMany(x => x.UserAuthorizationPolicies)
               .HasForeignKey(c => c.UserId)
               .OnDelete(DeleteBehavior.NoAction);

            builder
               .HasOne(c => c.GrantedBy)
               .WithMany(x => x.GrantedUserAuthorizationPolicies)
               .HasForeignKey(c => c.GrantedById)
               .OnDelete(DeleteBehavior.NoAction);

            builder
               .HasOne(c => c.AuthorizationPolicy)
               .WithMany(x => x.AuthorizationPolicyUsers)
               .HasForeignKey(c => c.AuthorizationPolicyId)
               .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
