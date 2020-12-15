namespace Camino.Service.Projections.Identity
{
    public class UserRoleProjection
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
