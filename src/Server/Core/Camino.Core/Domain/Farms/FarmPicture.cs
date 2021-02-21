using Camino.Core.Domain.Media;

namespace Camino.Core.Domain.Farms
{
    public class FarmPicture
    {
        public long Id { get; set; }
        public long FarmId { get; set; }
        public long PictureId { get; set; }
        public int PictureTypeId { get; set; }
        public virtual Farm Farm { get; set; }
        public virtual Picture Picture { get; set; }
    }
}
