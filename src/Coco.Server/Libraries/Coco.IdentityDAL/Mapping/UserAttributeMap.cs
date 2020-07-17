using Coco.Core.Constants;
using Coco.Core.Infrastructure.MapBuilders;
using Coco.Core.Entities.Identity;
using LinqToDB.Mapping;

namespace Coco.IdentityDAL.Mapping
{
    public class UserAttributeMap : EntityMapBuilder<UserAttribute>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<UserAttribute>().HasTableName(nameof(UserAttribute))
                .HasSchemaName(TableSchemaConst.DBO)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id);
        }
    }
}
