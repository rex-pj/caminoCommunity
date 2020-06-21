using Coco.Common.Const;
using Coco.Contract.MapBuilder;
using Coco.Entities.Domain.Content;
using LinqToDB.Mapping;

namespace Coco.DAL.Mapping
{
    public class UserPhotoMap : EntityTypeBuilder<UserPhoto>
    {
        public UserPhotoMap(FluentMappingBuilder fluentMappingBuilder) : base(fluentMappingBuilder)
        {
        }

        public override void Configure(FluentMappingBuilder builder)
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
