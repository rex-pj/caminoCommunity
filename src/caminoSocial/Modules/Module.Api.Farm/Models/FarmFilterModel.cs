using Camino.Framework.Models;

namespace Module.Api.Farm.Models
{
    public class FarmFilterModel : BaseFilterModel
    {
        public FarmFilterModel() : base()
        {

        }

        public long Id { get; set; }
        public string UserIdentityId { get; set; }
    }
}
