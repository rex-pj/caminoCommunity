using Camino.Framework.Models;
using System.Collections.Generic;

namespace Module.Api.Product.Models
{
    public class CreateProductModel
    {
        public CreateProductModel()
        {
            Pictures = new List<PictureRequestModel>();
            Farms = new List<ProductFarmModel>();
            Categories = new List<ProductCategoryRelationModel>();
            ProductAttributes = new List<ProductAttributeRelationModel>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        
        public decimal Price { get; set; }
        public IEnumerable<ProductCategoryRelationModel> Categories { get; set; }
        public IEnumerable<ProductFarmModel> Farms { get; set; }
        public IEnumerable<PictureRequestModel> Pictures { get; set; }
        public IEnumerable<ProductAttributeRelationModel> ProductAttributes { get; set; }
    }
}
