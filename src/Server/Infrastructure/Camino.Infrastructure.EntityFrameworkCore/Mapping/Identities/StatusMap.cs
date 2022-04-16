using Camino.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Camino.Core.Domain.Identifiers;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camino.Infrastructure.EntityFrameworkCore.Mapping.Identities
{
    public class StatusMap : IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> builder)
        {
            builder.ToTable(nameof(Status), TableSchemaConst.Auth);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder
               .HasMany(c => c.Users)
               .WithOne(x => x.Status)
               .HasForeignKey(c => c.StatusId);
        }
    }
}
