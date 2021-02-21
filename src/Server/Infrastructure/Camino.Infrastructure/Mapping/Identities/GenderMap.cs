using Camino.Core.Constants;
using Camino.Infrastructure.MapBuilders;
using Camino.Core.Domain.Identifiers;
using LinqToDB.Mapping;

namespace Camino.Infrastructure.Mapping.Identities
{
    public class GenderMap : EntityMapBuilder<Gender>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder
                .Entity<Gender>()
                .HasTableName(nameof(Gender))
                .HasSchemaName(TableSchemaConst.Auth)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id)
                .Association(x => x.UserInfos, (gender, userInfo) => gender.Id == userInfo.GenderId);
        }
    }
}
