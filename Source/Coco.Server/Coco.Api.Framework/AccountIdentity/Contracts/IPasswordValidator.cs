using Coco.Api.Framework.Models;
using System.Threading.Tasks;

namespace Coco.Api.Framework.AccountIdentity.Contracts
{
    public interface IPasswordValidator<TUser> where TUser : class
    {
        Task<ApiResult> ValidateAsync(IAccountManager<ApplicationUser> manager, ApplicationUser user, string password);
    }
}
