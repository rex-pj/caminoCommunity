using System.Collections.Generic;

namespace Module.Api.Feed.Models
{
    public class AdvancedSearchResultModel
    {
        public AdvancedSearchResultModel()
        {
            Articles = new List<FeedModel>();
            Products = new List<FeedModel>();
            Farms = new List<FeedModel>();
            Users = new List<FeedModel>();
            Communities = new List<FeedModel>();
        }

        public IList<FeedModel> Articles { get; set; }
        public int TotalArticle { get; set; }
        public int TotalArticlePage { get; set; }
        public IList<FeedModel> Products { get; set; }
        public int TotalProduct { get; set; }
        public int TotalProductPage { get; set; }
        public IList<FeedModel> Farms { get; set; }
        public int TotalFarm { get; set; }
        public int TotalFarmPage { get; set; }
        public IList<FeedModel> Users { get; set; }
        public int TotalUser { get; set; }
        public int TotalUserPage { get; set; }
        public IList<FeedModel> Communities { get; set; }
        public string UserFilterByName { get; set; }
        public int Page { get; set; }
    }
}
