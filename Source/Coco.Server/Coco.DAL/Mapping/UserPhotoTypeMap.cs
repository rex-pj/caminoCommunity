using Coco.Common.Const;
using Coco.Contract.MapBuilder;
using Coco.Entities.Domain.Content;
using LinqToDB.Mapping;

namespace Coco.DAL.Mapping
{
    public class UserPhotoTypeMap : EntityTypeBuilder<UserPhotoType>
    {
        public UserPhotoTypeMap(FluentMappingBuilder fluentMappingBuilder) : base(fluentMappingBuilder)
        {
        }

        public override void Configure(FluentMappingBuilder builder)
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
