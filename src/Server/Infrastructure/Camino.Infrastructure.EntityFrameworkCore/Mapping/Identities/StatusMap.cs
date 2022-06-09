using Camino.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Camino.Core.Domains.Users;

namespace Camino.Infrastructure.EntityFrameworkCore.Mapping.Identities
{
    public class StatusMap : IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> builder)
        {
            builder.ToTable(nameof(Status), TableSchemas.Auth);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder
               .HasMany(c => c.Users)
               .WithOne(x => x.Status)
               .HasForeignKey(c => c.StatusId);
        }
    }
}
