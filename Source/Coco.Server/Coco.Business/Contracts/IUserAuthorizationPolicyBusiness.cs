using Coco.Core.Dtos.Identity;

namespace Coco.Business.Contracts
{
    public interface IUserAuthorizationPolicyBusiness
    {
        bool Add(long userId, short authorizationPolicyId, long loggedUserId);
        bool Delete(long userId, short authorizationPolicyId);
        AuthorizationPolicyUsersDto GetAuthoricationPolicyUsers(short id);
    }
}
