using Camino.Shared.Enums;
using Camino.Shared.Requests.Providers;
using System.Threading.Tasks;

namespace Camino.Core.Contracts.Providers
{
    public interface IEmailProvider
    {
        Task SendEmailAsync(MailMessageRequest email, EmailTextFormat messageFormat = EmailTextFormat.Plain);
    }
}
