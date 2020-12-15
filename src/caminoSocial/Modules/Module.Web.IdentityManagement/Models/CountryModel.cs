using Camino.Framework.Models;

namespace Module.Web.IdentityManagement.Models
{
    public class CountryModel : BaseModel
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
