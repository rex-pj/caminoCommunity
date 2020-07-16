using Coco.Core.Constants;
using Coco.Core.Infrastructure.MapBuilders;
using Coco.Core.Entities.Content;
using LinqToDB.Mapping;

namespace Coco.DAL.Mapping
{
    public class UserPhotoTypeMap : EntityMapBuilder<UserPhotoType>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<UserPhotoType>()
                .HasTableName(nameof(UserPhotoType))
                .HasSchemaName(TableSchemaConst.DBO)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id)
                .Association(x => x.UserPhotos, (userPhotoType, userPhotos) => userPhotoType.Id == userPhotos.TypeId);
        }
    }
}
