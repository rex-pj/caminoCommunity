using Camino.Business.Dtos.General;
using Camino.Business.Dtos.Identity;
using System.Threading.Tasks;

namespace Camino.Business.Contracts
{
    public interface IUserAuthorizationPolicyBusiness
    {
        bool Add(long userId, long authorizationPolicyId, long loggedUserId);
        bool Delete(long userId, short authorizationPolicyId);
        AuthorizationPolicyUsersDto GetAuthoricationPolicyUsers(long id, UserAuthorizationPolicyFilterDto filter);
        Task<UserAuthorizationPolicyDto> GetUserAuthoricationPolicyAsync(long userId, long policyId);
        Task<bool> IsUserHasAuthoricationPolicyAsync(long userId, long policyId);
    }
}
