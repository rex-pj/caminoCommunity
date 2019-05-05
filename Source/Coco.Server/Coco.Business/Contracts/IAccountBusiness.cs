using Coco.Entities.Model.Account;
using System.Threading.Tasks;

namespace Coco.Business.Contracts
{
    public interface IAccountBusiness
    {
        long Add(UserModel user);
        Task<UserModel> Find(long id);
        Task<UserModel> FindUserByEmail(string email);
        void Delete(long id);
        bool Update(UserModel user);
    }
}
