namespace Coco.Entities.Model.General
{
    public class UpdateAvatarModel
    {
        public string PhotoUrl { get; set; }
        public int XAxis { get; set; }
        public int YAxis { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string ContentType { get; set; }
        public bool CanEdit { get; set; }
    }
}
