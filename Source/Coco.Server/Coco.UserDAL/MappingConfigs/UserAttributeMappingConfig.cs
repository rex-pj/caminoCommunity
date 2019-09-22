using Coco.Entities.Constant;
using Coco.Entities.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coco.IdentityDAL.MappingConfigs
{
    public class UserAttributeMappingConfig : IEntityTypeConfiguration<UserAttribute>
    {
        public void Configure(EntityTypeBuilder<UserAttribute> builder)
        {
            builder.ToTable(nameof(UserAttribute), TableSchemaConst.DBO);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}
