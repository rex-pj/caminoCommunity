using Coco.Framework.Models;
using System.Threading.Tasks;

namespace Coco.Framework.SessionManager.Contracts
{
    public interface IUserEmailStore<TUser> where TUser : class
    {
        Task<string> GetEmailAsync(ApplicationUser user);
        Task<ICommonResult> SendForgotPasswordAsync(ApplicationUser user);
        Task<bool> GetEmailConfirmedAsync(TUser user);
    }
}
