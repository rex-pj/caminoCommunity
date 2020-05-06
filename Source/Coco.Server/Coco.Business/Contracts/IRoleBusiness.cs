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
        RoleDto GetByName(string name);
        bool Update(RoleDto roleModel);
        Task<List<RoleDto>> GetAsync();
    }
}