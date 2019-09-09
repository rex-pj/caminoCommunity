using Coco.Api.Framework.SessionManager.Entities;
using Coco.Api.Framework.Models;
using System.Threading.Tasks;

namespace Coco.Api.Framework.SessionManager.Contracts
{
    public interface IUserValidator<TUser> where TUser : class
    {
        Task<ApiResult> ValidateAsync(IUserManager<TUser> manager, TUser user);
    }
}
