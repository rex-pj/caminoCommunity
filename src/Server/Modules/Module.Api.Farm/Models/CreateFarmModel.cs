using Camino.Infrastructure.AspNetCore.Models;
using System.Collections.Generic;

namespace Module.Api.Farm.Models
{
    public class CreateFarmModel
    {
        public CreateFarmModel()
        {
            Pictures = new List<PictureRequestModel>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public long FarmTypeId { get; set; }
        public string Address { get; set; }
        public IEnumerable<PictureRequestModel> Pictures { get; set; }
    }
}
