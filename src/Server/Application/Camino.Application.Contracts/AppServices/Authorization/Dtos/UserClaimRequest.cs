namespace Camino.Application.Contracts.AppServices.Authorization.Dtos
{
    public class UserClaimRequest
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
