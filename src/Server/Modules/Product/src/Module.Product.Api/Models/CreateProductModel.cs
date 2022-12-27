﻿using System.Collections.Generic;

namespace Module.Product.Api.Models
{
    public class CreateProductModel
    {
        public CreateProductModel()
        {
            Farms = new List<ProductFarmModel>();
            Categories = new List<ProductCategoryRelationModel>();
            ProductAttributes = new List<AttributeRelationRequestModel>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        
        public decimal Price { get; set; }
        public IEnumerable<ProductCategoryRelationModel> Categories { get; set; }
        public IEnumerable<ProductFarmModel> Farms { get; set; }
        public IEnumerable<AttributeRelationRequestModel> ProductAttributes { get; set; }
    }
}
