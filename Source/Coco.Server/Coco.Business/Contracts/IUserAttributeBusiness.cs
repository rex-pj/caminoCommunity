using Coco.Entities.Domain.Identity;
using System.Threading.Tasks;

namespace Coco.Business.Contracts
{
    public interface IUserAttributeBusiness
    {
        Task<int> CreateOrUpdateAsync(long userId, string key, string value);
        Task<UserAttribute> GetAsync(long userId, string key);
    }
}
