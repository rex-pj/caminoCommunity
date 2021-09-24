﻿using Camino.Framework.Models;
using System;
using System.Collections.Generic;

namespace Module.Api.Product.Models
{
    public class ProductModel
    {
        public ProductModel()
        {
            Pictures = new List<PictureResultModel>();
            Farms = new List<ProductFarmModel>();
            Categories = new List<ProductCategoryRelationModel>();
            ProductAttributes = new List<AttributeRelationResultModel>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public string CreatedByIdentityId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public long CreatedById { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public long UpdatedById { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByPhotoCode { get; set; }
        public decimal Price { get; set; }
        public IEnumerable<ProductCategoryRelationModel> Categories { get; set; }
        public IEnumerable<ProductFarmModel> Farms { get; set; }
        public IEnumerable<PictureResultModel> Pictures { get; set; }
        public IEnumerable<AttributeRelationResultModel> ProductAttributes { get; set; }
    }
}
