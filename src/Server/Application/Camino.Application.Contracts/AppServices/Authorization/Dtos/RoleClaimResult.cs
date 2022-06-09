namespace Camino.Application.Contracts.AppServices.Authorization.Dtos
{
    public class RoleClaimResult
    {
        public int Id { get; set; }
        public long RoleId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
