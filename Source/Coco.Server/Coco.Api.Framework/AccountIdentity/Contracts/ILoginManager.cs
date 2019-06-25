using Coco.Api.Framework.Models;
using System.Threading.Tasks;

namespace Coco.Api.Framework.AccountIdentity.Contracts
{
    public interface ILoginManager<TUser> where TUser : class
    {
        Task<ApiResult> LoginAsync(string userName, string password);
    }
}
