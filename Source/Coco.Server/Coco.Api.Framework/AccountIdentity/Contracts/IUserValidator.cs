using Coco.Api.Framework.AccountIdentity.Entities;
using Coco.Api.Framework.Models;
using System.Threading.Tasks;

namespace Coco.Api.Framework.AccountIdentity.Contracts
{
    public interface IUserValidator<TUser> where TUser : class
    {
        Task<ApiResult> ValidateAsync(IAccountManager<TUser> manager, TUser user);
    }
}
