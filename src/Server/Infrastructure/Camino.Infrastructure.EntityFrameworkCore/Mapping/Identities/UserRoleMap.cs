using Camino.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Camino.Core.Domains.Authorization;

namespace Camino.Infrastructure.EntityFrameworkCore.Mapping.Identities
{
    public class UserRoleMap : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable(nameof(UserRole), TableSchemas.Auth);
            builder.HasKey(x => new
            {
                x.UserId,
                x.RoleId
            });

            builder
                .HasOne(c => c.User)
                .WithMany(x => x.UserRoles)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(c => c.Role)
                .WithMany(x => x.UserRoles)
                .HasForeignKey(c => c.RoleId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(c => c.GrantedBy)
                .WithMany(x => x.GrantedUserRoles)
                .HasForeignKey(c => c.GrantedById)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
