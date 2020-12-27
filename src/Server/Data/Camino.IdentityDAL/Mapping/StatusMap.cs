using Camino.Core.Constants;
using Camino.Data.MapBuilders;
using LinqToDB.Mapping;
using Camino.IdentityDAL.Entities;

namespace Camino.IdentityDAL.Mapping
{
    public class StatusMap : EntityMapBuilder<Status>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<Status>().HasTableName(nameof(Status))
                .HasSchemaName(TableSchemaConst.DBO)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id)
                .Association(c => c.Users, (status, users) => status.Id == users.StatusId);
        }
    }
}
