namespace Camino.Framework.Models
{
    public class MailMessageModel
    {
        public string FromEmail { get; set; }
        public string FromName { get; set; }

        public string ToEmail { get; set; }
        public string ToName { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }
    }
}
