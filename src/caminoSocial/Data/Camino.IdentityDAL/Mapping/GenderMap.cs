using Camino.Core.Constants;
using Camino.Data.MapBuilders;
using Camino.IdentityDAL.Entities;
using LinqToDB.Mapping;

namespace Camino.IdentityDAL.Mapping
{
    public class GenderMap : EntityMapBuilder<Gender>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder
                .Entity<Gender>()
                .HasTableName(nameof(Gender))
                .HasSchemaName(TableSchemaConst.DBO)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id)
                .Association(x => x.UserInfos, (gender, userInfo) => gender.Id == userInfo.GenderId);
        }
    }
}
