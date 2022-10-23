using Camino.Infrastructure.AspNetCore.Models;
using System.Collections.Generic;

namespace Module.Api.Feed.Models
{
    public class FeedPageListModel : PageListModel<FeedModel>
    {
        public FeedPageListModel(IEnumerable<FeedModel> collections) : base(collections)
        {
        }
    }
}
