using Camino.Infrastructure.Identity.Models;

namespace Module.Web.IdentityManagement.Models
{
    public class CountryModel : BaseIdentityModel
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
