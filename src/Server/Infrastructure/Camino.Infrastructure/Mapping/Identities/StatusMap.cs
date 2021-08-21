using Camino.Infrastructure.Commons.Constants;
using Camino.Infrastructure.MapBuilders;
using LinqToDB.Mapping;
using Camino.Core.Domain.Identifiers;

namespace Camino.Infrastructure.Mapping.Identities
{
    public class StatusMap : EntityMapBuilder<Status>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<Status>().HasTableName(nameof(Status))
                .HasSchemaName(TableSchemaConst.Auth)
                .HasPrimaryKey(x => x.Id)
                .HasIdentity(x => x.Id)
                .Association(c => c.Users, (status, users) => status.Id == users.StatusId);
        }
    }
}
