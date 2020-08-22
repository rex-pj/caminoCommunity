using Camino.Framework.Models;
using MimeKit.Text;
using System.Threading.Tasks;

namespace Camino.Framework.Providers.Contracts
{
    public interface IEmailProvider
    {
        Task SendEmailAsync(MailMessageModel email, TextFormat messageFormat = TextFormat.Plain);
    }
}
