using Coco.Api.Framework.AccountIdentity.Entities;
using System;
using System.Threading.Tasks;

namespace Coco.Api.Framework.AccountIdentity.Contracts
{
    public interface IAccountManager<TUser> : IDisposable where TUser : class
    {
        IdentityOptions Options { get; set; }
        Task<string> GetUserNameAsync(TUser user);
        Task<string> GetEmailAsync(TUser user);
    }
}
