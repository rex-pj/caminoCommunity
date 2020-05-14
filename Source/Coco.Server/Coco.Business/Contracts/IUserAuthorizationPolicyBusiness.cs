using Coco.Entities.Dtos.Auth;

namespace Coco.Business.Contracts
{
    public interface IUserAuthorizationPolicyBusiness
    {
        bool Add(long userId, short authorizationPolicyId, long loggedUserId);
        bool Delete(long userId, short authorizationPolicyId);
        AuthorizationPolicyUsersDto GetAuthoricationPolicyUsers(short id);
    }
}
