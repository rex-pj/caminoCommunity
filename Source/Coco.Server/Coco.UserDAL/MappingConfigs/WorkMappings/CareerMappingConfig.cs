using Coco.Entities.Constant;
using Coco.Entities.Domain.Work;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coco.UserDAL.MappingConfigs.WorkMappings
{
    public class CareerMappingConfig : IEntityTypeConfiguration<Career>
    {
        public void Configure(EntityTypeBuilder<Career> builder)
        {
            builder.ToTable(nameof(Career), TableSchemaConst.WORK);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.HasMany(c => c.UserCareers)
               .WithOne(x => x.Career)
               .HasForeignKey(c => c.CareerId);
        }
    }
}
