using Camino.Framework.Models;

namespace Module.Api.Farm.Models
{
    public class FarmFilterModel : BaseFilterModel
    {
        public FarmFilterModel() : base()
        {
            Page = 1;
            PageSize = 10;
        }

        public long Id { get; set; }
        public string UserIdentityId { get; set; }
    }
}
