using Camino.Core.Domains.Media;
using Microsoft.EntityFrameworkCore;
using Camino.Shared.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camino.Infrastructure.EntityFrameworkCore.Mapping.Media
{
    public class UserPhotoMap : IEntityTypeConfiguration<UserPhoto>
    {
        public void Configure(EntityTypeBuilder<UserPhoto> builder)
        {
            builder.ToTable(nameof(UserPhoto), TableSchemas.Dbo);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder
               .HasOne(c => c.UserPhotoType)
               .WithMany(x => x.UserPhotos)
               .HasForeignKey(c => c.TypeId);
        }
    }
}
