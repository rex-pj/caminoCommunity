using Coco.Framework.Models;
using System.Threading.Tasks;

namespace Coco.Framework.SessionManager.Contracts
{
    public interface IUserValidator<TUser> where TUser : class
    {
        Task<ICommonResult> ValidateAsync(IUserManager<TUser> manager, TUser user);
    }
}
