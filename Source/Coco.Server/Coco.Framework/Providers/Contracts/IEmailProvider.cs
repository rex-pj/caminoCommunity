using Coco.Framework.Models;
using MimeKit.Text;
using System.Threading.Tasks;

namespace Coco.Framework.Providers.Contracts
{
    public interface IEmailProvider
    {
        Task SendEmailAsync(MailMessageModel email, TextFormat messageFormat = TextFormat.Plain);
    }
}
