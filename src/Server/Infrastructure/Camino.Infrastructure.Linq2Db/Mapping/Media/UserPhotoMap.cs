using LinqToDB.Mapping;
using Camino.Core.Domain.Media;
using Camino.Infrastructure.Linq2Db.MapBuilders;
using Camino.Shared.Constants;

namespace Camino.Infrastructure.Linq2Db.Mapping.Media
{
    public class UserPhotoMap : EntityMapBuilder<UserPhoto>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<UserPhoto>()
                .HasTableName(nameof(UserPhoto))
                .HasSchemaName(TableSchemaConst.Dbo)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id)
                .Association(x => x.UserPhotoType, (userPhoto, type) => userPhoto.TypeId == type.Id);
        }
    }
}
