using Camino.Infrastructure.Emails.Contracts.Dtos;
using Camino.Shared.Enums;
using System.Threading.Tasks;

namespace Camino.Infrastructure.Emails.Contracts
{
    public interface IEmailProvider
    {
        Task SendEmailAsync(MailMessageRequest email, EmailTextFormats messageFormat = EmailTextFormats.Plain);
    }
}
