namespace Camino.Application.Contracts.AppServices.Authorization.Dtos
{
    public class UserRoleRequest
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }
    }
}
