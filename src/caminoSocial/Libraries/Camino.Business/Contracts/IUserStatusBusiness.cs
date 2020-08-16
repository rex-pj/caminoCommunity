using Camino.Business.Dtos.Identity;
using System.Collections.Generic;

namespace Camino.Business.Contracts
{
    public interface IUserStatusBusiness
    {
        IList<UserStatusDto> Search(string query = "", int page = 1, int pageSize = 10);
        List<UserStatusDto> GetAll();
        UserStatusDto Find(int id);
        UserStatusDto FindByName(string name);
        int Add(UserStatusDto statusDto);
        UserStatusDto Update(UserStatusDto statusDto);
    }
}
