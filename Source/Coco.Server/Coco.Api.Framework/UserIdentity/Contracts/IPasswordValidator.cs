using Coco.Api.Framework.Models;
using System.Threading.Tasks;

namespace Coco.Api.Framework.UserIdentity.Contracts
{
    public interface IPasswordValidator<TUser> where TUser : class
    {
        Task<ApiResult> ValidateAsync(IUserManager<ApplicationUser> manager, ApplicationUser user, string password);
    }
}
