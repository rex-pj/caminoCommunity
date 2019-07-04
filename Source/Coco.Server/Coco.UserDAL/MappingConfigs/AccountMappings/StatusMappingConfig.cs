using Coco.Entities.Constant;
using Coco.Entities.Domain.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coco.UserDAL.MappingConfigs.AccountMappings
{
    public class StatusMappingConfig : IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> builder)
        {
            builder.ToTable(nameof(Status), TableSchemaConst.DBO);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.HasMany(x => x.Users)
                .WithOne(x => x.Status)
                .HasForeignKey(x => x.StatusId);
        }
    }
}
