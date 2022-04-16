using Microsoft.EntityFrameworkCore;
using Camino.Core.Domain.Identifiers;
using Camino.Shared.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camino.Infrastructure.EntityFrameworkCore.Mapping.Identities
{
    public class UserInfoMap : IEntityTypeConfiguration<UserInfo>
    {
        public void Configure(EntityTypeBuilder<UserInfo> builder)
        {
            builder.ToTable(nameof(UserInfo), TableSchemaConst.Auth);
            builder.HasKey(x => x.Id);

            builder
               .HasOne(c => c.User)
               .WithOne(x => x.UserInfo)
               .HasForeignKey<User>(c => c.Id);

            builder
               .HasOne(c => c.Gender)
               .WithMany(x => x.UserInfos)
               .HasForeignKey(c => c.GenderId);

            builder
               .HasOne(c => c.Country)
               .WithMany(x => x.UserInfos)
               .HasForeignKey(c => c.CountryId);
        }
    }
}
