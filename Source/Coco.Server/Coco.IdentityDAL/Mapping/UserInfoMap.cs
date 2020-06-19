using Coco.Common.Const;
using Coco.Contract.MapBuilder;
using Coco.Entities.Domain.Identity;
using LinqToDB.Mapping;

namespace Coco.IdentityDAL.Mapping
{
    public class UserInfoMap : EntityTypeBuilder<UserInfo>
    {
        public UserInfoMap(FluentMappingBuilder fluentMappingBuilder) : base(fluentMappingBuilder)
        {
        }

        public override void Configure(FluentMappingBuilder builder)
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
