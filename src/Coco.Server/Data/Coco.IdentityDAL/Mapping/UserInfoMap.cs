using Coco.Core.Constants;
using Coco.Data.MapBuilders;
using Coco.Data.Entities.Identity;
using LinqToDB.Mapping;

namespace Coco.IdentityDAL.Mapping
{
    public class UserInfoMap : EntityMapBuilder<UserInfo>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<UserInfo>()
                .HasTableName(nameof(UserInfo))
                .HasSchemaName(TableSchemaConst.DBO)
                .HasPrimaryKey(x => x.Id)
                .Association(e => e.User, (userInfo, user) => userInfo.Id == user.Id)
                .Association(e => e.Gender, (user, gender) => user.GenderId == gender.Id)
                .Association(e => e.Country, (user, country) => user.CountryId == country.Id);
        }
    }
}
