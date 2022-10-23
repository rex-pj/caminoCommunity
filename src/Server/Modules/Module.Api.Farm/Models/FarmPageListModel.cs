using Camino.Infrastructure.AspNetCore.Models;
using System.Collections.Generic;

namespace Module.Api.Farm.Models
{
    public class FarmPageListModel : PageListModel<FarmModel>
    {
        public FarmPageListModel(IEnumerable<FarmModel> collections) : base(collections)
        {
        }
    }
}
