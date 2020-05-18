using Coco.Common.Const;
using Coco.Entities.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coco.IdentityDAL.Mapping
{
    public class GenderMap : IEntityTypeConfiguration<Gender>
    {
        public void Configure(EntityTypeBuilder<Gender> builder)
        {
            builder.ToTable(nameof(Gender), TableSchemaConst.DBO);
            builder.HasKey(x => x.Id);
            builder.Property<byte>(x => x.Id).ValueGeneratedOnAdd();

            builder
               .HasMany(c => c.UserInfos)
               .WithOne(x => x.Gender)
               .HasForeignKey(c => c.GenderId)
               .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
