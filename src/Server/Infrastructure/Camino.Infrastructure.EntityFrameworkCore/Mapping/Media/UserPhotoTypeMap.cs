using Camino.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Camino.Core.Domain.Media;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camino.Infrastructure.EntityFrameworkCore.Mapping.Media
{
    public class UserPhotoTypeMap : IEntityTypeConfiguration<UserPhotoType>
    {
        public void Configure(EntityTypeBuilder<UserPhotoType> builder)
        {
            builder.ToTable(nameof(UserPhotoType), TableSchemaConst.Dbo);
            builder.HasKey(x => x.Id);
            builder.Property<byte>(x => x.Id).ValueGeneratedOnAdd();

            builder
               .HasMany(c => c.UserPhotos)
               .WithOne(x => x.UserPhotoType)
               .HasForeignKey(c => c.TypeId);
        }
    }
}
