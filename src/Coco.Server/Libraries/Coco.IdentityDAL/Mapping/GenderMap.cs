using Coco.Core.Constants;
using Coco.Core.Infrastructure.MapBuilders;
using Coco.Core.Entities.Identity;
using LinqToDB.Mapping;

namespace Coco.IdentityDAL.Mapping
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
