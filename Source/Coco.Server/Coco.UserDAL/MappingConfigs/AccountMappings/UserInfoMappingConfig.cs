using Coco.Entities.Constant;
using Coco.Entities.Domain.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coco.UserDAL.MappingConfigs.AccountMappings
{
    public class UserInfoMappingConfig : IEntityTypeConfiguration<UserInfo>
    {
        public void Configure(EntityTypeBuilder<UserInfo> builder)
        {
            builder.ToTable(nameof(UserInfo), TableSchemaConst.ACCOUNT);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasKey(x => x.Id);
        }
    }
}
