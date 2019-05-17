using Coco.Api.Framework.AccountIdentity.Entities;
using Coco.Api.Framework.Models;
using System.Threading.Tasks;

namespace Coco.Api.Framework.AccountIdentity.Contracts
{
    public interface IPasswordValidator<TUser> where TUser : class
    {
        Task<IdentityResult> ValidateAsync(IAccountManager<ApplicationUser> manager, ApplicationUser user, string password);
    }
}
