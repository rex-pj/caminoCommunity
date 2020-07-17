using Camino.Core.Constants;
using Camino.Data.MapBuilders;
using Camino.Data.Entities.Identity;
using LinqToDB.Mapping;

namespace Camino.IdentityDAL.Mapping
{
    public class CountryMap : EntityMapBuilder<Country>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<Country>()
                .HasTableName(nameof(Country))
                .HasSchemaName(TableSchemaConst.DBO)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id)
                .Association(c => c.UserInfos, (country, userInfos) => country.Id == userInfos.CountryId);
        }
    }
}
