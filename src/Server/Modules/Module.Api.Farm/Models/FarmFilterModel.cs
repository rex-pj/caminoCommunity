using Camino.Framework.Models;
using HotChocolate;
using HotChocolate.Types;

namespace Module.Api.Farm.Models
{
    public class FarmFilterModel : BaseFilterModel
    {
        public FarmFilterModel() : base()
        {
            Page = 1;
            PageSize = 10;
        }

        [GraphQLType(typeof(LongType))]
        public long Id { get; set; }
        public string ExclusiveUserIdentityId { get; set; }
        public string UserIdentityId { get; set; }
    }
}
