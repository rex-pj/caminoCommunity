using Coco.Api.Framework.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Coco.Api.Framework.SessionManager.Contracts
{
    public interface IUserEmailStore<TUser> where TUser : class
    {
        Task<string> GetEmailAsync(ApplicationUser user);
        Task<ApiResult<string>> SendForgotPasswordAsync(ApplicationUser user);
        Task<bool> GetEmailConfirmedAsync(TUser user);
    }
}
