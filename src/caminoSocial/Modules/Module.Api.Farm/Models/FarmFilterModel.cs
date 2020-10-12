using Camino.Framework.Models;

namespace Module.Api.Farm.Models
{
    public class FarmFilterModel : BaseFilterModel
    {
        public FarmFilterModel() : base()
        {

        }

        public string UserIdentityId { get; set; }
    }
}
