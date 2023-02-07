﻿using Camino.Infrastructure.AspNetCore.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        public IEnumerable<ProductCategoryRelationModel> Categories { get; set; }
        public IEnumerable<ProductFarmModel> Farms { get; set; }
        public IEnumerable<PictureRequestModel> Pictures { get; set; }
        public IEnumerable<AttributeRelationRequestModel> ProductAttributes { get; set; }
    }
}
