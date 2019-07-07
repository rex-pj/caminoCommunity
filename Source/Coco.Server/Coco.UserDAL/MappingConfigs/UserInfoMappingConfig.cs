using Coco.Entities.Constant;
using Coco.Entities.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coco.IdentityDAL.MappingConfigs
{
    public class UserInfoMappingConfig : IEntityTypeConfiguration<UserInfo>
    {
        public void Configure(EntityTypeBuilder<UserInfo> builder)
        {
            builder.ToTable(nameof(UserInfo), TableSchemaConst.DBO);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasKey(x => x.Id);
        }
    }
}
