using Coco.Api.Framework.AccountIdentity.Entities;
using System.Threading.Tasks;

namespace Coco.Api.Framework.AccountIdentity.Contracts
{
    public interface IUserValidator<TUser> where TUser : class
    {
        Task<IdentityResult> ValidateAsync(IAccountManager<TUser> manager, TUser user);
    }
}
