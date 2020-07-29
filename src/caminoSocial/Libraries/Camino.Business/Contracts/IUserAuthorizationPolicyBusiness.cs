using Camino.Business.Dtos.Identity;

namespace Camino.Business.Contracts
{
    public interface IUserAuthorizationPolicyBusiness
    {
        bool Add(long userId, short authorizationPolicyId, long loggedUserId);
        bool Delete(long userId, short authorizationPolicyId);
        AuthorizationPolicyUsersDto GetAuthoricationPolicyUsers(short id);
    }
}
