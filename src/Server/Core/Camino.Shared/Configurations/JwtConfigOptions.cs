namespace Camino.Shared.Configurations
{
    public class JwtConfigOptions
    {
        public const string Name = "JwtConfigOptions";
        public string SecretKey { get; set; }
        public int HourExpires { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
