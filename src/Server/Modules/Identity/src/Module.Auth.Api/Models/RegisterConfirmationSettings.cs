namespace  Module.Auth.Api.Models
{
    public class RegisterConfirmationSettings
    {
        public const string Name = "RegisterConfirmation";
        public string Url { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
    }
}
