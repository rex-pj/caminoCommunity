using Camino.Framework.Models;

namespace Module.Web.IdentityManagement.Models
{
    public class UserStatusModel : BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
