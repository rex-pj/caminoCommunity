namespace Coco.Framework.Models
{
    public class CocoSettings
    {
        public const string Name = "Coco";
        public string ApplicationName { get; set; }
        public string MyAllowSpecificOrigins { get; set; }
        public string[] AllowOrigins { get; set; }
    }
}
