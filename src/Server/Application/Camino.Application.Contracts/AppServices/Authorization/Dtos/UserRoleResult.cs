namespace Camino.Application.Contracts.AppServices.Authorization.Dtos
{
    public class UserRoleResult
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
