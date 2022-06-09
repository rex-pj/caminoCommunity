namespace Camino.Application.Contracts.AppServices.Authorization.Dtos
{
    public class UserAuthorizationPolicyResult
    {
        public long UserId { get; set; }
        public long AuthorizationPolicyId { get; set; }
    }
}
