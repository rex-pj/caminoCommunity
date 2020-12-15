using Camino.Service.Projections.Filters;
using Camino.Service.Projections.Identity;
using Camino.Service.Projections.PageList;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camino.Service.Business.Authorization.Contracts
{
    public interface IRoleBusiness
    {
        Task<long> AddAsync(RoleProjection roleModel);
        Task<bool> DeleteAsync(long id);
        Task<RoleProjection> FindAsync(long id);
        Task<RoleProjection> GetByNameAsync(string name);
        Task<bool> UpdateAsync(RoleProjection roleModel);
        Task<BasePageList<RoleProjection>> GetAsync(RoleFilter filter);
        RoleProjection FindByName(string name);
        Task<RoleProjection> FindByNameAsync(string name);
        List<RoleProjection> Search(string query = "", List<long> currentRoleIds = null, int page = 1, int pageSize = 10);
    }
}