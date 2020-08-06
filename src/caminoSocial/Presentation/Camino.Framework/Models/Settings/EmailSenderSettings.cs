namespace Camino.Framework.Models.Settings
{
    public class EmailSenderSettings
    {
        public const string Name = "EmailSender";
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
