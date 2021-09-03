using Camino.Infrastructure.MapBuilders;
using Camino.Core.Domain.Identifiers;
using LinqToDB.Mapping;
using Camino.Infrastructure.Commons.Constants;

namespace Camino.Infrastructure.Mapping.Identities
{
    public class UserInfoMap : EntityMapBuilder<UserInfo>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<UserInfo>()
                .HasTableName(nameof(UserInfo))
                .HasSchemaName(TableSchemaConst.Auth)
                .HasPrimaryKey(x => x.Id)
                .Association(e => e.User, (userInfo, user) => userInfo.Id == user.Id)
                .Association(e => e.Gender, (user, gender) => user.GenderId == gender.Id)
                .Association(e => e.Country, (user, country) => user.CountryId == country.Id);
        }
    }
}
