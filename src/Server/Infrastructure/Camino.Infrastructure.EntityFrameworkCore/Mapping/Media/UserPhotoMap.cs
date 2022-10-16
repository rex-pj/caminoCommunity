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
            builder.Property(x => x.Description)
                .IsRequired(false);

            builder.Property(x => x.Name)
                .IsRequired();
            builder.Property(x => x.CreatedDate)
                .IsRequired();
            builder.Property(x => x.CreatedById)
                .IsRequired();
            builder.Property(x => x.FileData)
                .IsRequired();
            builder.Property(x => x.UserId)
                .IsRequired();
            builder.Property(x => x.TypeId)
                .IsRequired();

            builder
               .HasOne(c => c.UserPhotoType)
               .WithMany(x => x.UserPhotos)
               .HasForeignKey(c => c.TypeId);
        }
    }
}
