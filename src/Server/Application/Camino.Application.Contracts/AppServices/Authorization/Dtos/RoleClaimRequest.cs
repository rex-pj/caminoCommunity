namespace Camino.Application.Contracts.AppServices.Authorization.Dtos
{
    public class RoleClaimRequest
    {
        public int Id { get; set; }
        public long RoleId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
