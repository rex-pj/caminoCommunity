using Camino.Business.Dtos.General;
using Camino.Business.Dtos.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camino.Business.Contracts
{
    public interface IRoleBusiness
    {
        Task<long> AddAsync(RoleDto roleModel);
        Task<bool> DeleteAsync(long id);
        Task<RoleDto> FindAsync(long id);
        Task<RoleDto> GetByNameAsync(string name);
        Task<bool> UpdateAsync(RoleDto roleModel);
        Task<PageListDto<RoleDto>> GetAsync(RoleFilterDto filter);
        RoleDto FindByName(string name);
        Task<RoleDto> FindByNameAsync(string name);
        List<RoleDto> Search(string query = "", List<long> currentRoleIds = null, int page = 1, int pageSize = 10);
    }
}