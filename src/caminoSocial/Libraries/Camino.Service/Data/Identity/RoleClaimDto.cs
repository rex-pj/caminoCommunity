namespace Camino.Service.Data.Identity
{
    public class RoleClaimDto
    {
        public int Id { get; set; }
        public long RoleId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
