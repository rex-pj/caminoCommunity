using Camino.Framework.Models;

namespace Module.Api.Feed.Models
{
    public class FeedFilterModel : BaseFilterModel
    {
        public FeedFilterModel() : base()
        {
            Page = 1;
            PageSize = 10;
        }

        public string UserIdentityId { get; set; }
    }
}
