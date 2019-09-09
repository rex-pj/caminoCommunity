using System;
using System.Collections.Generic;
using System.Text;

namespace Coco.Api.Framework.Models
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
