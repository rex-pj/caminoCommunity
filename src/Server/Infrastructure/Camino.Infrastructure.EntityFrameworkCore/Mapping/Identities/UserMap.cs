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
            builder.Property(x => x.Address).IsRequired(false);
            builder.Property(x => x.BirthDate).IsRequired(false);
            builder.Property(x => x.Description).IsRequired(false);
            builder.Property(x => x.GenderId).IsRequired(false);
            builder.Property(x => x.CountryId).IsRequired(false);
            builder.Property(x => x.PhoneNumber).IsRequired(false);
            builder.Property(x => x.CreatedById).IsRequired(false);
            builder.Property(x => x.UpdatedById).IsRequired(false);

            builder.Property(x => x.Email).IsRequired();
            builder.Property(x => x.UserName).IsRequired();
            builder.Property(x => x.Lastname).IsRequired();
            builder.Property(x => x.Firstname).IsRequired();
            builder.Property(x => x.DisplayName).IsRequired();
            builder.Property(x => x.PasswordHash).IsRequired();
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.Property(x => x.UpdatedDate).IsRequired();
            builder.Property(x => x.SecurityStamp).IsRequired();

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
               .HasForeignKey(x => x.GrantedById);

            builder
               .HasOne(c => c.Gender)
               .WithMany(x => x.Users)
               .HasForeignKey(c => c.GenderId);

            builder
               .HasOne(c => c.Country)
               .WithMany(x => x.Users)
               .HasForeignKey(c => c.CountryId);
        }
    }
}
