namespace Camino.Infrastructure.Identity.Options
{
    public class JwtConfigOptions
    {
        public const string Name = "JwtConfigOptions";
        public string SecretKey { get; set; }
        public double HourExpires { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public double RefreshTokenHourExpires { get; set; }
    }
}
