using Coco.Api.Framework.Models;
using System.Threading.Tasks;

namespace Coco.Api.Framework.SessionManager.Contracts
{
    public interface IUserTokenProvider<TUser> where TUser : ApplicationUser
    {
        Task<string> GenerateAsync(string purpose, IUserManager<TUser> manager, TUser user);
        Task<bool> CanGenerateTwoFactorTokenAsync(IUserManager<TUser> manager, TUser user);
        Task<bool> ValidateAsync(string purpose, string token, IUserManager<TUser> manager, TUser user);
    }
}
