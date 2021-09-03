using Camino.Shared.Requests.Filters;
using Camino.Shared.Requests.Authorization;
using Camino.Shared.Results.PageList;
using System.Collections.Generic;
using System.Threading.Tasks;
using Camino.Shared.Results.Authorization;

namespace Camino.Core.Contracts.Repositories.Authorization
{
    public interface IRoleRepository
    {
        Task<long> CreateAsync(RoleModifyRequest request);
        Task<bool> DeleteAsync(long id);
        Task<RoleResult> FindAsync(long id);
        Task<RoleResult> GetByNameAsync(string name);
        Task<bool> UpdateAsync(RoleModifyRequest request);
        Task<BasePageList<RoleResult>> GetAsync(RoleFilter filter);
        RoleResult FindByName(string name);
        Task<RoleResult> FindByNameAsync(string name);
        List<RoleResult> Search(BaseFilter filter, List<long> currentRoleIds = null);
    }
}