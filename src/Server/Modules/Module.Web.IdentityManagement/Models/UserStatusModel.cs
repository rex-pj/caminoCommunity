using Camino.Infrastructure.Identity.Models;

namespace Module.Web.IdentityManagement.Models
{
    public class UserStatusModel : BaseIdentityModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
