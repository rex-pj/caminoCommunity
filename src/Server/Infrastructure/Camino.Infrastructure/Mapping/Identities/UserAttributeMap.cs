using Camino.Infrastructure.MapBuilders;
using Camino.Core.Domain.Identifiers;
using LinqToDB.Mapping;
using Camino.Infrastructure.Commons.Constants;

namespace Camino.Infrastructure.Mapping.Identities
{
    public class UserAttributeMap : EntityMapBuilder<UserAttribute>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<UserAttribute>().HasTableName(nameof(UserAttribute))
                .HasSchemaName(TableSchemaConst.Auth)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id);
        }
    }
}
