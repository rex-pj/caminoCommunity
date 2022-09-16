using Camino.Shared.Enums;

namespace Camino.Application.Contracts.AppServices.Feeds.Dtos
{
    public class FeedResult
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? PictureId { get; set; }
        public long? CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedByPhotoCode { get; set; }
        public string? Address { get; set; }
        public decimal? Price { get; set; }
        public FeedTypes FeedType { get; set; }
    }
}
