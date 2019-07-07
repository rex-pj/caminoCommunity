using Coco.Entities.Constant;
using Coco.Entities.Domain.Work;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coco.IdentityDAL.MappingConfigs
{
    public class UserCareerMappingConfig : IEntityTypeConfiguration<UserCareer>
    {
        public void Configure(EntityTypeBuilder<UserCareer> builder)
        {
            builder.ToTable(nameof(UserCareer), TableSchemaConst.DBO);
            builder.HasKey(c => new { c.UserId, c.CareerId });
        }
    }
}
