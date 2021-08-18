using Camino.Framework.Models;

namespace Module.Api.Feed.Models
{
    public class FeedFilterModel : BaseFilterModel
    {
        public FeedFilterModel() : base()
        {
        }

        public int? HoursCreatedFrom { get; set; }
        public int? HoursCreatedTo { get; set; }
        public string UserIdentityId { get; set; }
        public int? FilterType { get; set; }
    }
}
