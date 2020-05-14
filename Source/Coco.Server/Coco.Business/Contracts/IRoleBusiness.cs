using Coco.Entities.Dtos.Auth;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coco.Business.Contracts
{
    public interface IRoleBusiness
    {
        byte Add(RoleDto roleModel);
        bool Delete(byte id);
        RoleDto Find(byte id);
        Task<RoleDto> GetByNameAsync(string name);
        bool Update(RoleDto roleModel);
        Task<List<RoleDto>> GetAsync();
        RoleDto FindByName(string name);
        List<RoleDto> Search(string query = "", int page = 1, int pageSize = 10);
    }
}