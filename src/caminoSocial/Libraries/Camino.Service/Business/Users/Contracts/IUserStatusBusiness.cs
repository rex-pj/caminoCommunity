using Camino.Service.Data.Filters;
using Camino.Service.Data.Identity;
using Camino.Service.Data.Page;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camino.Service.Business.Users.Contracts
{
    public interface IUserStatusBusiness
    {
        IList<UserStatusResult> Search(string query = "", int page = 1, int pageSize = 10);
        Task<PageList<UserStatusResult>> GetAsync(UserStatusFilter filter);
        UserStatusResult Find(int id);
        UserStatusResult FindByName(string name);
        int Add(UserStatusResult statusDto);
        UserStatusResult Update(UserStatusResult statusDto);
    }
}
