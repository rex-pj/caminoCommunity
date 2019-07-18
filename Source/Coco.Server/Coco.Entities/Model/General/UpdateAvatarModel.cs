namespace Coco.Entities.Model.General
{
    public class UpdateAvatarModel
    {
        public string PhotoUrl { get; set; }
        public double XAxis { get; set; }
        public double YAxis { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public string ContentType { get; set; }
        public bool CanEdit { get; set; }
    }
}
