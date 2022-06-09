using Camino.Core.Domains.Users;
using Camino.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camino.Infrastructure.EntityFrameworkCore.Mapping.Identities
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .ToTable(nameof(User), TableSchemas.Auth)
                .HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder
                .HasOne(x => x.UserInfo)
                .WithOne(x => x.User)
                .HasForeignKey<UserInfo>(x => x.Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(x => x.CreatedBy)
                .WithMany(x => x.CreatedUsers)
                .HasForeignKey(x => x.CreatedById)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(x => x.UpdatedBy)
                .WithMany(x => x.UpdatedUsers)
                .HasForeignKey(x => x.UpdatedById)
                .OnDelete(DeleteBehavior.NoAction);

            builder
               .HasMany(x => x.UserRoles)
               .WithOne(x => x.User)
               .HasForeignKey(x => x.UserId);

            builder
               .HasMany(x => x.CreatedRoles)
               .WithOne(x => x.CreatedBy)
               .HasForeignKey(x => x.CreatedById);

            builder
               .HasMany(x => x.UpdatedRoles)
               .WithOne(x => x.UpdatedBy)
               .HasForeignKey(x => x.UpdatedById);

            builder
               .HasOne(x => x.Status)
               .WithMany(x => x.Users)
               .HasForeignKey(x => x.StatusId);

            builder
               .HasMany(x => x.CreatedAuthorizationPolicies)
               .WithOne(x => x.CreatedBy)
               .HasForeignKey(x => x.CreatedById);

            builder
               .HasMany(x => x.UpdatedAuthorizationPolicies)
               .WithOne(x => x.UpdatedBy)
               .HasForeignKey(x => x.UpdatedById);

            builder
               .HasMany(x => x.UserAuthorizationPolicies)
               .WithOne(x => x.User)
               .HasForeignKey(x => x.UserId);

            builder
               .HasMany(x => x.GrantedRoleAuthorizationPolicies)
               .WithOne(x => x.GrantedBy)
               .HasForeignKey(x => x.GrantedBy);
        }
    }
}
