using Camino.Service.Projections.Filters;
using Camino.Service.Projections.Identity;
using Camino.Service.Projections.PageList;
using System.Threading.Tasks;

namespace Camino.Service.Business.Authorization.Contracts
{
    public interface IUserAuthorizationPolicyBusiness
    {
        bool Create(long userId, long authorizationPolicyId, long loggedUserId);
        bool Delete(long userId, short authorizationPolicyId);
        AuthorizationPolicyUsersPageList GetAuthoricationPolicyUsers(long id, UserAuthorizationPolicyFilter filter);
        Task<UserAuthorizationPolicyProjection> GetUserAuthoricationPolicyAsync(long userId, long policyId);
        Task<bool> IsUserHasAuthoricationPolicyAsync(long userId, long policyId);
    }
}
