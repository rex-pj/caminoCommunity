using Coco.Api.Framework.Models;
using MimeKit.Text;
using System.Threading.Tasks;

namespace Coco.Api.Framework.Services.Contracts
{
    public interface IEmailSender
    {
        Task SendEmailAsync(MailMessageModel email, TextFormat messageFormat = TextFormat.Plain);
    }
}
