using Camino.Infrastructure.AspNetCore.Models;
using HotChocolate;
using HotChocolate.Types;
using System.Collections.Generic;

namespace Module.Product.Api.Models
{
    public class UpdateProductModel
    {
        public UpdateProductModel()
        {
            Pictures = new List<PictureRequestModel>();
            Farms = new List<ProductFarmModel>();
            Categories = new List<ProductCategoryRelationModel>();
            ProductAttributes = new List<AttributeRelationRequestModel>();
        }

        [GraphQLType(typeof(LongType))]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public IEnumerable<ProductCategoryRelationModel> Categories { get; set; }
        public IEnumerable<ProductFarmModel> Farms { get; set; }
        public IEnumerable<PictureRequestModel> Pictures { get; set; }
        public IEnumerable<AttributeRelationRequestModel> ProductAttributes { get; set; }
    }
}
