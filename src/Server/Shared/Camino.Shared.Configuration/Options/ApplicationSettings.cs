namespace Camino.Shared.Configuration.Options
{
    public class ApplicationSettings
    {
        public const string Name = "Application";
        public string ApplicationName { get; set; }
        public string MyAllowSpecificOrigins { get; set; }
        public string[] AllowOrigins { get; set; }
        public string CaminoClientAppUrl { get; set; }
        public int MaxUploadFileSize { get; set; }
    }
}
