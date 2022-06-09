using Microsoft.EntityFrameworkCore;
using Camino.Shared.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Camino.Core.Domains.Users;

namespace Camino.Infrastructure.EntityFrameworkCore.Mapping.Identities
{
    public class UserAttributeMap : IEntityTypeConfiguration<UserAttribute>
    {
        public void Configure(EntityTypeBuilder<UserAttribute> builder)
        {
            builder.ToTable(nameof(UserAttribute), TableSchemas.Auth);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}
