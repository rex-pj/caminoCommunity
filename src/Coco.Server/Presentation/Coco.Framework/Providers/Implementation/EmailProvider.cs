using Coco.Framework.Models;
using Coco.Framework.Providers.Contracts;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System;
using System.Threading.Tasks;

namespace Coco.Framework.Providers.Implementation
{
    public class EmailProvider : IEmailProvider
    {
        private readonly EmailSenderSettings _emailSenderSettings;
        public EmailProvider(IOptions<EmailSenderSettings> emailSenderSettings)
        {
            _emailSenderSettings = emailSenderSettings.Value;
        }

        public async Task SendEmailAsync(MailMessageModel email, TextFormat messageFormat = TextFormat.Plain)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(email.FromName, email.FromEmail));
            message.To.Add(new MailboxAddress(email.ToName, email.ToEmail));
            message.Subject = email.Subject;

            message.Body = new TextPart(messageFormat)
            {
                Text = email.Body
            };

            try
            {
                using (var client = new SmtpClient())
                {
                    // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    await client.ConnectAsync(_emailSenderSettings.SmtpServer, _emailSenderSettings.SmtpPort, false);

                    // Note: only needed if the SMTP server requires authentication
                    await client.AuthenticateAsync(_emailSenderSettings.UserName, _emailSenderSettings.Password);

                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
