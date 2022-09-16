using Camino.Shared.Enums;

namespace Camino.Application.Contracts.AppServices.Feeds.Dtos
{
    public class FeedFilter : BaseFilter
    {
        public DateTime? CreatedDateFrom { get; set; }
        public DateTime? CreatedDateTo { get; set; }
        public long? CreatedById { get; set; }
        public bool CanGetInactived { get; set; }
        public bool CanGetDeleted { get; set; }
        public FeedFilterTypes? FilterType { get; set; }
    }
}
