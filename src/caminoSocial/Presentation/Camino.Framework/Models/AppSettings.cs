namespace Camino.Framework.Models
{
    public class AppSettings
    {
        public const string Name = "Camino";
        public string ApplicationName { get; set; }
        public string MyAllowSpecificOrigins { get; set; }
        public string[] AllowOrigins { get; set; }
    }
}
