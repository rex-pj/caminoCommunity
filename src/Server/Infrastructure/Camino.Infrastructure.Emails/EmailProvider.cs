using Camino.Shared.Enums;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using Camino.Infrastructure.Emails.Contracts;
using Camino.Infrastructure.Emails.Contracts.Dtos;
using Camino.Core.DependencyInjection;
using Camino.Shared.Configuration.Options;
using System.Threading.Tasks;
using System;

namespace Camino.Infrastructure.Providers
{
    public class EmailProvider : IEmailProvider, IScopedDependency
    {
        private readonly EmailSenderSettings _emailSenderSettings;
        public EmailProvider(IOptions<EmailSenderSettings> emailSenderSettings)
        {
            _emailSenderSettings = emailSenderSettings.Value;
        }

        public async Task SendEmailAsync(MailMessageRequest email, EmailTextFormats messageFormat = EmailTextFormats.Plain)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(email.FromName, email.FromEmail));
            message.To.Add(new MailboxAddress(email.ToName, email.ToEmail));
            message.Subject = email.Subject;

            var textFormatNumber = (int)messageFormat;
            message.Body = new TextPart((TextFormat)textFormatNumber)
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
