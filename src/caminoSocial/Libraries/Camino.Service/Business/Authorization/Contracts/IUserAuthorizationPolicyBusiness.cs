using Camino.Service.Data.Filters;
using Camino.Service.Data.Identity;
using Camino.Service.Data.PageList;
using System.Threading.Tasks;

namespace Camino.Service.Business.Authorization.Contracts
{
    public interface IUserAuthorizationPolicyBusiness
    {
        bool Add(long userId, long authorizationPolicyId, long loggedUserId);
        bool Delete(long userId, short authorizationPolicyId);
        AuthorizationPolicyUsersPageList GetAuthoricationPolicyUsers(long id, UserAuthorizationPolicyFilter filter);
        Task<UserAuthorizationPolicyProjection> GetUserAuthoricationPolicyAsync(long userId, long policyId);
        Task<bool> IsUserHasAuthoricationPolicyAsync(long userId, long policyId);
    }
}
