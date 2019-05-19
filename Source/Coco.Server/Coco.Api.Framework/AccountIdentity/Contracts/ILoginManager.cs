using Coco.Api.Framework.AccountIdentity.Entities;
using System.Threading.Tasks;

namespace Coco.Api.Framework.AccountIdentity.Contracts
{
    public interface ILoginManager<TUser> where TUser : class
    {
        Task<LoginResult> LoginAsync(string userName, string password);
    }
}
