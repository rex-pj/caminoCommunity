using Camino.Framework.Models;
using HotChocolate;
using HotChocolate.Types;
using System.Collections.Generic;

namespace Module.Api.Farm.Models
{
    public class UpdateFarmModel
    {
        public UpdateFarmModel()
        {
            Pictures = new List<PictureRequestModel>();
        }

        [GraphQLType(typeof(LongType))]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long FarmTypeId { get; set; }
        public string Address { get; set; }
        public IEnumerable<PictureRequestModel> Pictures { get; set; }
    }
}
