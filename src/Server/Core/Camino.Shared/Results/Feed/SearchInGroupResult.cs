using System.Collections.Generic;

namespace Camino.Shared.Results.Feed
{
    public class SearchInGroupResult
    {
        public SearchInGroupResult()
        {
            Articles = new List<FeedResult>();
            Products = new List<FeedResult>();
            Farms = new List<FeedResult>();
            Users = new List<FeedResult>();
            Communities = new List<FeedResult>();
        }

        public IList<FeedResult> Articles { get; set; }
        public int TotalArticle { get; set; }
        public int TotalArticlePage { get; set; }
        public IList<FeedResult> Products { get; set; }
        public int TotalProduct { get; set; }
        public int TotalProductPage { get; set; }
        public IList<FeedResult> Farms { get; set; }
        public int TotalFarm { get; set; }
        public int TotalFarmPage { get; set; }
        public IList<FeedResult> Users { get; set; }
        public int TotalUser { get; set; }
        public int TotalUserPage { get; set; }
        public IList<FeedResult> Communities { get; set; }
    }
}
