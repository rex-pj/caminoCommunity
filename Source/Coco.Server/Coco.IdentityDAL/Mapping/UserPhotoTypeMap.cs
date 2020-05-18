using Coco.Common.Const;
using Coco.Entities.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coco.IdentityDAL.Mapping
{
    public class UserPhotoTypeMap : IEntityTypeConfiguration<UserPhotoType>
    {
        public void Configure(EntityTypeBuilder<UserPhotoType> builder)
        {
            builder.ToTable(nameof(UserPhotoType), TableSchemaConst.DBO);
            builder.HasKey(x => x.Id);
            builder.Property<byte>(x => x.Id).ValueGeneratedOnAdd();

            builder
               .HasMany(c => c.UserPhotos)
               .WithOne(x => x.Type)
               .HasForeignKey(c => c.TypeId);
        }
    }
}
