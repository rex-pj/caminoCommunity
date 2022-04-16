using Microsoft.EntityFrameworkCore;
using Camino.Core.Domain.Identifiers;
using Camino.Shared.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camino.Infrastructure.EntityFrameworkCore.Mapping.Identities
{
    public class UserLoginMap : IEntityTypeConfiguration<UserLogin>
    {
        public void Configure(EntityTypeBuilder<UserLogin> builder)
        {
            builder.ToTable(nameof(UserLogin), TableSchemaConst.Auth);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.ProviderDisplayName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.LoginProvider)
               .IsRequired()
               .HasMaxLength(255);

            builder.Property(x => x.ProviderKey)
                .IsRequired();

            builder
               .HasOne(c => c.User)
               .WithMany(x => x.UserLogins)
               .HasForeignKey(c => c.UserId);
        }
    }
}
