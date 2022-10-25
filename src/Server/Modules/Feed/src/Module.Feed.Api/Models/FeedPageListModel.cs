using Camino.Infrastructure.AspNetCore.Models;
using System.Collections.Generic;

namespace Module.Feed.Api.Models
{
    public class FeedPageListModel : PageListModel<FeedModel>
    {
        public FeedPageListModel(IEnumerable<FeedModel> collections) : base(collections)
        {
        }
    }
}
