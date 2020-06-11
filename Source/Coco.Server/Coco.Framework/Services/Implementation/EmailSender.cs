using Coco.Common.Const;
using Coco.Framework.Models;
using Coco.Framework.Services.Contracts;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using System;
using System.Threading.Tasks;

namespace Coco.Framework.Services.Implementation
{
    public class EmailSender : IEmailSender
    {
        private readonly string _host;
        private readonly int _port;
        private readonly string _userName;
        private readonly string _Password;
        public EmailSender(IConfiguration configuration) {
            _host = configuration[ConfigurationSettingsConst.EMAIL_SENDER_SMTP_SERVER];
            _port = int.Parse(configuration[ConfigurationSettingsConst.EMAIL_SENDER_SMTP_PORT]);
            _userName = configuration[ConfigurationSettingsConst.EMAIL_SENDER_USER_NAME];
            _Password = configuration[ConfigurationSettingsConst.EMAIL_SENDER_PASSWORD];
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

                    await client.ConnectAsync(_host, _port, false);

                    // Note: only needed if the SMTP server requires authentication
                    await client.AuthenticateAsync(_userName, _Password);

                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
