using LinqToDB.Mapping;
using Camino.Infrastructure.Linq2Db.MapBuilders;
using Camino.Core.Domain.Farms;
using Camino.Shared.Constants;

namespace Camino.Infrastructure.Linq2Db.Mapping.Farms
{
    public class FarmPictureMap : EntityMapBuilder<FarmPicture>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<FarmPicture>()
                .HasTableName(nameof(FarmPicture))
                .HasSchemaName(TableSchemaConst.Dbo)
                .HasPrimaryKey(x => x.Id)
                .HasIdentity(x => x.Id)
                .Association(x => x.Picture, 
                    (farmPicture, picture) => farmPicture.PictureId == picture.Id)
                .Association(x => x.Farm, 
                    (farmPicture, farm) => farmPicture.FarmId == farm.Id);
        }
    }
}
