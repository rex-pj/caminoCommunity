using Coco.Api.Framework.UserIdentity.Entities;
using Coco.Api.Framework.Models;
using System.Threading.Tasks;

namespace Coco.Api.Framework.UserIdentity.Contracts
{
    public interface IUserValidator<TUser> where TUser : class
    {
        Task<ApiResult> ValidateAsync(IUserManager<TUser> manager, TUser user);
    }
}
