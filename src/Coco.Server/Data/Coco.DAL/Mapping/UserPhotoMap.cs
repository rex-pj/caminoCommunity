using Coco.Core.Constants;
using Coco.Data.Entities.Content;
using LinqToDB.Mapping;
using Coco.Data.MapBuilders;

namespace Coco.DAL.Mapping
{
    public class UserPhotoMap : EntityMapBuilder<UserPhoto>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<UserPhoto>()
                .HasTableName(nameof(UserPhoto))
                .HasSchemaName(TableSchemaConst.DBO)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id)
                .Association(x => x.Type, (userPhoto, type) => userPhoto.TypeId == type.Id);
        }
    }
}
