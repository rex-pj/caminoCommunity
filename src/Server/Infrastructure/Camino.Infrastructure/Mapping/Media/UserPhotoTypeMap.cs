using Camino.Core.Constants;
using Camino.Infrastructure.MapBuilders;
using Camino.Core.Domain.Media;
using LinqToDB.Mapping;

namespace Camino.Infrastructure.Mapping.Media
{
    public class UserPhotoTypeMap : EntityMapBuilder<UserPhotoType>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<UserPhotoType>()
                .HasTableName(nameof(UserPhotoType))
                .HasSchemaName(TableSchemaConst.Dbo)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id)
                .Association(x => x.UserPhotos, (userPhotoType, userPhotos) => userPhotoType.Id == userPhotos.TypeId);
        }
    }
}
