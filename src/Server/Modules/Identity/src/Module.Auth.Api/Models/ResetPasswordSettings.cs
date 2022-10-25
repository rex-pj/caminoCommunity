namespace  Module.Auth.Api.Models
{
    public class ResetPasswordSettings
    {
        public const string Name = "ResetPassword";
        public string Url { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
    }
}
