namespace Camino.Shared.Configurations
{
    public class AppSettings
    {
        public const string Name = "App";
        public string ApplicationName { get; set; }
        public string MyAllowSpecificOrigins { get; set; }
        public string[] AllowOrigins { get; set; }
        public string CaminoClientAppUrl { get; set; }

    }
}
