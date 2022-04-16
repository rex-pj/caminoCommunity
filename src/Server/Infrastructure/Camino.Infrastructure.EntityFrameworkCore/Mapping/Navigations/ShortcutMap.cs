using Camino.Core.Domain.Navigations;
using Camino.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camino.Infrastructure.EntityFrameworkCore.Mapping.Navigations
{
    public class ShortcutMap : IEntityTypeConfiguration<Shortcut>
    {
        public void Configure(EntityTypeBuilder<Shortcut> builder)
        {
            builder.ToTable(nameof(Shortcut), TableSchemaConst.Dbo);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}
