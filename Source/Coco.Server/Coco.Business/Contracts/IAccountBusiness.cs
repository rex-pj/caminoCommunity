using Coco.Entities.Model.Account;
using System.Threading.Tasks;

namespace Coco.Business.Contracts
{
    public interface IAccountBusiness
    {
        long Add(UserModel user);
        Task<UserModel> Find(long id);
        Task<UserModel> FindUserByEmail(string email, bool includeInActived = false);
        Task<UserModel> FindUserByUsername(string username, bool includeInActived = false);
        void Delete(long id);
        Task<UserModel> UpdateAsync(UserModel user);
    }
}
