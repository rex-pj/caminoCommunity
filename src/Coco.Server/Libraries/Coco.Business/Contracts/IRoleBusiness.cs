using Coco.Core.Dtos.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coco.Business.Contracts
{
    public interface IRoleBusiness
    {
        Task<long> AddAsync(RoleDto roleModel);
        Task<bool> DeleteAsync(long id);
        Task<RoleDto> FindAsync(long id);
        Task<RoleDto> GetByNameAsync(string name);
        Task<bool> UpdateAsync(RoleDto roleModel);
        Task<List<RoleDto>> GetAsync();
        RoleDto FindByName(string name);
        Task<RoleDto> FindByNameAsync(string name);
        List<RoleDto> Search(string query = "", int page = 1, int pageSize = 10);
    }
}