using Camino.Service.Data.Filters;
using Camino.Service.Data.Identity;
using Camino.Service.Data.Page;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camino.Service.Business.Authorization.Contracts
{
    public interface IRoleBusiness
    {
        Task<long> AddAsync(RoleResult roleModel);
        Task<bool> DeleteAsync(long id);
        Task<RoleResult> FindAsync(long id);
        Task<RoleResult> GetByNameAsync(string name);
        Task<bool> UpdateAsync(RoleResult roleModel);
        Task<PageList<RoleResult>> GetAsync(RoleFilter filter);
        RoleResult FindByName(string name);
        Task<RoleResult> FindByNameAsync(string name);
        List<RoleResult> Search(string query = "", List<long> currentRoleIds = null, int page = 1, int pageSize = 10);
    }
}