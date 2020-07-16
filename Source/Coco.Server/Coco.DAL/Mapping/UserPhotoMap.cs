﻿using Coco.Core.Constants;
using Coco.Core.Infrastructure.MapBuilders;
using Coco.Core.Entities.Content;
using LinqToDB.Mapping;

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
