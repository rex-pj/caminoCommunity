using Camino.Core.Constants;
using LinqToDB.Mapping;
using Camino.Data.MapBuilders;
using Camino.DAL.Entities;

namespace Camino.DAL.Mapping
{
    public class FarmPictureMap : EntityMapBuilder<FarmPicture>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<FarmPicture>()
                .HasTableName(nameof(FarmPicture))
                .HasSchemaName(TableSchemaConst.DBO)
                .HasPrimaryKey(x => x.Id)
                .HasIdentity(x => x.Id)
                .Association(x => x.Picture, 
                    (farmPicture, picture) => farmPicture.PictureId == picture.Id)
                .Association(x => x.Farm, 
                    (farmPicture, farm) => farmPicture.FarmId == farm.Id);
        }
    }
}
