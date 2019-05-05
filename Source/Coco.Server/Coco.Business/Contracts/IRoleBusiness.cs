using Coco.Entities.Model.Auth;

namespace Coco.Business.Contracts
{
    public interface IRoleBusiness
    {
        byte Add(RoleModel roleModel);
        bool Delete(byte id);
        RoleModel Find(byte id);
        RoleModel GetByName(string name);
        bool Update(RoleModel roleModel);
    }
}