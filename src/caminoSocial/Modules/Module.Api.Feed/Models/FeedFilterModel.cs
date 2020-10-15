using Camino.Framework.Models;

namespace Module.Api.Feed.Models
{
    public class FeedFilterModel : BaseFilterModel
    {
        public FeedFilterModel() : base()
        {

        }

        public string UserIdentityId { get; set; }
    }
}
