namespace Camino.DAL.Entities
{
    public class FarmPicture
    {
        public long Id { get; set; }
        public long FarmId { get; set; }
        public long PictureId { get; set; }
        public int PictureType { get; set; }
        public virtual Farm Farm { get; set; }
        public virtual Picture Picture { get; set; }
    }
}
