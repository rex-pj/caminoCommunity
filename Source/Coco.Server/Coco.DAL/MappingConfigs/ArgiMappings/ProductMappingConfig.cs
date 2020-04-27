using Coco.Entities.Constant;
using Coco.Entities.Domain.Agri;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coco.DAL.MappingConfigs.ArgiMappings
{
    public class ProductMappingConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable(nameof(Product), TableSchemaConst.AGRICULTURE);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}
