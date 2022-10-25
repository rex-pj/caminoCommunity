using Camino.Infrastructure.AspNetCore.Models;

namespace Module.Farm.Api.Models
{
    public class FarmFilterModel : BaseFilterModel
    {
        public FarmFilterModel() : base()
        {
        }

        public long? Id { get; set; }
        public string ExclusiveUserIdentityId { get; set; }
        public string UserIdentityId { get; set; }
    }
}
