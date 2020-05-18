using Coco.Common.Const;
using Coco.Entities.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coco.IdentityDAL.Mapping
{
    public class StatusMap : IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> builder)
        {
            builder.ToTable(nameof(Status), TableSchemaConst.DBO);
            builder.HasKey(x => x.Id);
            builder.Property<byte>(x => x.Id).ValueGeneratedOnAdd();

            builder
               .HasMany(c => c.Users)
               .WithOne(x => x.Status)
               .HasForeignKey(c => c.StatusId);
        }
    }
}
