namespace Camino.Service.Projections.Identity
{
    public class RoleClaimProjection
    {
        public int Id { get; set; }
        public long RoleId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
